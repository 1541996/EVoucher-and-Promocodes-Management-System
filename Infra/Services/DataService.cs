using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.Services
{
    public struct AuthDataIndex
    {
        public const int Email = 0;
        public const int Name = 1;
        public const int ID = 2;
        public const int JobTitle = 3;
        public const int PhotoUrl = 4;
    }

    //public class DataService
    //{
    //    public static object DataList { get; private set; }

    //    public static tbEvent getCurrentEvent()
    //    {
    //        tbEvent evt = new tbEvent();
    //        evt.ID = CurrentEvent.eventid;
    //        evt.Title = CurrentEvent.name;
    //        evt.Photo = "mohs.png";
    //        evt.Description = "Auditorium (2), Advanced Molecular Research Centre";
    //        return evt;
    //    }
    //}
}
