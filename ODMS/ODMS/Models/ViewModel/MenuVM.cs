using System.Collections.Generic;

namespace ODMS.Models.ViewModel
{
    public class MenuiVm
    {
        public List<tbl_MainMenu> MainMenus { get; set; }
        public List<tbl_SubMenu> SubMenu { get; set; }
        public List<tbl_SubMenuSecond> SubMenuSecond { get; set; }

    }
}