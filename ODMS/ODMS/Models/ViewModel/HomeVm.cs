using System.Collections.Generic;

namespace ODMS.Models.ViewModel
{
    public class HomeVm
    {
        public int NoOfDb { get; set; }

        public int NoOfPSr { get; set; }

        public int NoOfLogin { get; set; }

        public List<HomekpiVm> Kpi{ get; set; }
    }


    public class HomekpiVm
    {
        public string Performar { get; set; }
        public int ScheduleCall { get; set; }
        public int ProductiveMemo { get; set; }
        public double StrikeRate { get; set; }
        public int Tlsd { get; set; }
        public double Lpsc { get; set; }
        public double MtdTarget { get; set; }
        public double Order { get; set; }
        public double Drop { get; set; }

    }



    
}