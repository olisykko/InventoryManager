using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace InventoryManager
{
    [ApiVersion(2, 1)]
    public class Plugin : TerrariaPlugin
    {
        public override string Author => "oli";
        public override string Name => "InventoryManager";
        public override Version Version => new("1.0");

        public Plugin(Main main) : base(main) { }
        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command("oli.invmanager", OnInventory, "inv") { AllowServer = false });
            DB.Setup();
        }
        static void OnInventory(CommandArgs e)
        {
            if (!e.Player.IsLoggedIn)
            {
                e.Player.SendErrorMessage("Войдите в аккаунт чтобы использовать команду.");
                return;
            }
            InventoryManager inventoryManager = new(e.Player);
            switch (e.Parameters.Count < 1 ? "help" : e.Parameters[0].ToLower())
            {
                case "help":
                    e.Player.SendErrorMessage("Неверный формат! /inv <save/load/del/rename/list> <name/page>");
                    return;
                case "load":
                    {
                        if (e.Parameters.Count < 2)
                        {
                            e.Player.SendErrorMessage("Вы должны ввести название инвентаря!");
                            return;
                        }
                        string name = e.Parameters[1].ToLower();
                        if (inventoryManager.Load(name))
                            e.Player.SendSuccessMessage("Вы загрузили инвентарь '{0}'!", name);
                        else
                            e.Player.SendErrorMessage("Такого инвентаря не существует! '{0}'", name);
                    }
                    return;
                case "save":
                    {
                        if (e.Parameters.Count < 2)
                        {
                            e.Player.SendErrorMessage("Вы должны ввести название инвентаря!");
                            return;
                        }
                        string name = e.Parameters[1].ToLower();
                        if (inventoryManager.Save(name))
                            e.Player.SendSuccessMessage("Вы сохранили инвентарь '{0}'!", name);
                    }
                    return;
                case "list":
                    {
                        if (!e.Player.IsLoggedIn)
                        {
                            e.Player.SendErrorMessage("Войдите в аккаунт чтобы использовать команду.");
                        }
                        else
                        {
                            if (!PaginationTools.TryParsePageNumber(e.Parameters, 1, e.Player, out int page))
                                return;
                            var list = inventoryManager.GetPlayerInventories();
                            IEnumerable<string> inventories = from inventory in list select inventory.name;
                            PaginationTools.SendPage(e.Player, page, PaginationTools.BuildLinesFromTerms(inventories, maxCharsPerLine: 100), new()
                            {
                                HeaderFormat = "Список сохранённых инвентарей.",
                                FooterFormat = "Следующая страница /inv list {0}",
                                NothingToDisplayString = "У вас нет сохраненных инвентарей."
                            });
                        }
                    }
                    return;
                case "del":
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
                            e.Player.SendErrorMessage("Этого инвентаря нет в базе данных! '{0}'", name);
                    }
                    return;
                case "rename":
                    {
                        if (e.Parameters.Count < 3)
                        {
                            e.Player.SendErrorMessage("Неверный формат! /inv rename <old name> <new name>");
                            return;
                        }
                        string oldName = e.Parameters[1].ToLower();
                        string newName = e.Parameters[2].ToLower();
                        if (inventoryManager.Rename(oldName, newName, out var contains))
                            e.Player.SendSuccessMessage("Инвентарь успешно переименован! {0} на {1}", oldName, newName);
                        else if (contains)
                            e.Player.SendErrorMessage("У вас уже есть инвентарь с таким названием! {0}", newName);
                        else
                            e.Player.SendErrorMessage("Такого инвентаря не существует! {0}", newName);
                    }
                    return;
                default:
                    {
                        goto case "help";
                    }
            }
        }
    }
}
