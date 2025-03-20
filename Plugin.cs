using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace InventoryManager
{
    [ApiVersion(2, 1)]
    public class Plugin : TerrariaPlugin
    {
        public override string Author => "oli & SteelSeries";
        public override string Name => "InventoryManager";
        public override Version Version => new("1.5");

        public Plugin(Main main) : base(main) { }
        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command("oli.invmanager", OnInventory, "inv") { AllowServer = false });
            DB.Setup();
        }
        private static void OnInventory(CommandArgs e)
        {
            InventoryManager inventoryManager = new(e.Player);
            switch (e.Parameters.Count == 0 ? "help" : e.Parameters[0].ToLower())
            {
                case "help":
                    e.Player.SendInfoMessage("/inv save <Inventory Name> <Private? (true/false)> (Сохраняет инвентарь.)");
                    e.Player.SendInfoMessage("/inv load <Inventory Name> <Owner? (Account Name)> (Загружает ваш или чужой инвентарь.)");
                    e.Player.SendInfoMessage("/inv rename <Inventory Name> <New Inventory Name> (Переименует ваш инвентарь.)");
                    e.Player.SendInfoMessage("/inv del <Inventory Name> (Удаляет ваш инвентарь.)");
                    e.Player.SendInfoMessage("/inv list <Page?> (Список ваших инвентарей.)");
                    e.Player.SendInfoMessage("/inv listall <Page?> (Список ваших инвентарей или публичных.)");
                    e.Player.SendInfoMessage("/inv info <Inventory Name> <Owner? (Account Name)> (Информация о инвентаре.)");
                    e.Player.SendInfoMessage("/inv privacy <Inventory Name> (Изменяет публичность инвентаря.)");
                    return;
                case "load":
                    {
                        if (e.Parameters.Count < 2)
                        {
                            e.Player.SendErrorMessage("Вы должны ввести название инвентаря!");
                            return;
                        }
                        string name = e.Parameters[1].ToLower();
                        if (inventoryManager.Load(name, e.Parameters.IndexInRange(2) ? e.Parameters[2] : null))
                            e.Player.SendSuccessMessage("Вы загрузили инвентарь '{0}'!", name);
                    }
                    return;
                case "save":
                    {
                        if (e.Parameters.Count < 2)
                        {
                            e.Player.SendErrorMessage("Вы должны ввести название инвентаря!");
                            return;
                        }
                        if (!e.Player.IsLoggedIn)
                        {
                            e.Player.SendErrorMessage("Вы должны быть зарегистрированы, чтобы сохранять инвентари!");
                            return;
                        }
                        string name = e.Parameters[1].ToLower();
                        bool? setPrivate = e.Parameters.IndexInRange(2) ? (bool.TryParse(e.Parameters[2], out bool result) ? (bool?)result : null) : null;
                        if (inventoryManager.Save(name, setPrivate))
                            e.Player.SendSuccessMessage("Вы сохранили инвентарь '{0}'! Настройка приватности: {1}", name, inventoryManager.GetPlayerInventories().First(i => i.name == name).isPrivate);
                    }
                    return;
                case "list":
                    {
                        if (!PaginationTools.TryParsePageNumber(e.Parameters, 1, e.Player, out int page))
                            return;
                        if (!e.Player.IsLoggedIn)
                        {
                            e.Player.SendErrorMessage("Вы должны быть зарегистрированы, чтобы посмотреть список ваших инвентарей!");
                            return;
                        }
                        var inventoryList = inventoryManager.GetPlayerInventories().Select(i => i.name);
                        PaginationTools.SendPage(e.Player, page, PaginationTools.BuildLinesFromTerms(inventoryList, maxCharsPerLine: 100), new()
                        {
                            HeaderFormat = "Список ваших сохранённых инвентарей.",
                            FooterFormat = "Следующая страница /inv list {0}",
                            NothingToDisplayString = "Нет доступных инвентарей."
                        });
                    }
                    return;
                case "listall":
                    {
                        if (!PaginationTools.TryParsePageNumber(e.Parameters, 1, e.Player, out int page))
                            return;
                        var inventoryList = inventoryManager.GetPlayerInventories(true).Where(i => !i.isPrivate || i.owner == e.Player.Account?.Name).Select(i => i.name + $" (Owner: {i.owner})");
                        PaginationTools.SendPage(e.Player, page, PaginationTools.BuildLinesFromTerms(inventoryList, maxCharsPerLine: 100), new()
                        {
                            HeaderFormat = "Список сохранённых инвентарей.",
                            FooterFormat = "Следующая страница /inv listall {0}",
                            NothingToDisplayString = "Нет доступных инвентарей."
                        });
                    }
                    return;
                case "del":
                case "delete":
                case "remove":
                    {
                        if (e.Parameters.Count < 2)
                        {
                            e.Player.SendErrorMessage("Вы должны ввести название инвентаря!");
                            return;
                        }
                        string name = e.Parameters[1].ToLower();
                        if (inventoryManager.Delete(name))
                            e.Player.SendSuccessMessage("Вы удалили инвентарь '{0}'!", name);
                        else
                            e.Player.SendErrorMessage("Инвентарь '{0}' не найден!", name);
                    }
                    return;
                case "rename":
                    {
                        if (e.Parameters.Count < 3)
                        {
                            e.Player.SendErrorMessage("Неверный формат! /inv rename <Inventory Name> <New Inventory Name>");
                            return;
                        }
                        string oldName = e.Parameters[1].ToLower();
                        string newName = e.Parameters[2].ToLower();
                        if (inventoryManager.Rename(oldName, newName, out var contains))
                            e.Player.SendSuccessMessage("Инвентарь '{0}' успешно переименован: '{1}'!", oldName, newName);
                        else if (contains)
                            e.Player.SendErrorMessage("Инвентарь '{0}' уже существует!", newName);
                        else
                            e.Player.SendErrorMessage("Инвентарь '{0}' не найден!", oldName);
                    }
                    return;
                case "privacy":
                case "private":
                case "public":
                    {
                        if (e.Parameters.Count < 2)
                        {
                            e.Player.SendErrorMessage("Вы должны ввести название инвентаря!");
                            return;
                        }
                        if (!e.Player.IsLoggedIn)
                        {
                            e.Player.SendErrorMessage("Войдите в аккаунт чтобы использовать команду.");
                            return;
                        }
                        string name = e.Parameters[1].ToLower();
                        if (inventoryManager.SetPrivacy(name, out var isPrivate))
                            e.Player.SendSuccessMessage("Инвентарь '{0}' теперь {1}!", name, isPrivate ? "приватный" : "публичный");
                        else
                            e.Player.SendErrorMessage("Инвентарь '{0}' не найден! ", name);
                    }
                    return;
                case "info":
                    {
                        if (e.Parameters.Count < 2)
                        {
                            e.Player.SendErrorMessage("Вы должны ввести название инвентаря!");
                            return;
                        }
                        inventoryManager.ShowInfo(e.Parameters[1].ToLower(), e.Parameters.IndexInRange(2) ? e.Parameters[2] : null);
                    }
                    return;
                default:
                    goto case "help";
            }
        }
    }
}