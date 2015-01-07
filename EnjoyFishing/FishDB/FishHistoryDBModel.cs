﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFACETools;
using System.Xml.Serialization;
using System.Reflection;
using System.Collections;
using MiscTools;

namespace EnjoyFishing
{
    [XmlRoot("History")]
    public class FishHistoryDBModel
    {
        [XmlAttribute("player")]
        public string PlayerName { get; set; }
        [XmlAttribute("date")]
        public DateTime EarthDate { get; set; }
        public int CatchCount { get; set; }
        [XmlArray("Fishes")]
        [XmlArrayItem("Fish")]
        public List<FishHistoryDBFishModel> Fishes { get; set; }
        public FishHistoryDBModel()
        {
            this.PlayerName = string.Empty;
            this.EarthDate = DateTime.Today;
            this.CatchCount = 0;
            this.Fishes = new List<FishHistoryDBFishModel>();
            
        }
    }
    public class FishHistoryDBFishModel
    {
        [XmlAttribute("name")]
        public string FishName { get; set; }
        [XmlAttribute("count")]
        public int FishCount { get; set; }
        [XmlAttribute("zone")]
        public string ZoneName { get; set; }
        [XmlAttribute("rod")]
        public string RodName { get; set; }
        [XmlAttribute("bait")]
        public string BaitName { get; set; }
        [XmlAttribute("id1")]
        public int ID1 { get; set; }
        [XmlAttribute("id2")]
        public int ID2 { get; set; }
        [XmlAttribute("id3")]
        public int ID3 { get; set; }
        [XmlAttribute("id4")]
        public int ID4 { get; set; }
        [XmlAttribute("critical")]
        public bool Critical { get; set; }
        [XmlAttribute("type")]
        public FishDBFishTypeKind FishType { get; set; }
        [XmlAttribute("result")]
        public FishResultStatusKind Result { get; set; }
        [XmlAttribute("earthtime")]
        public DateTime EarthTime { get; set; }
        [XmlAttribute("vanatime")]
        public string VanaTime { get; set; }
        [XmlAttribute("weekday")]
        public Weekday VanaWeekDay { get; set; }
        [XmlAttribute("moon")]
        public MoonPhase MoonPhase { get; set; }
        [XmlAttribute("x")]
        public float X { get; set; }
        [XmlAttribute("y")]
        public float Y { get; set; }
        [XmlAttribute("z")]
        public float Z { get; set; }
        [XmlAttribute("h")]
        public float H { get; set; }
        public FishHistoryDBFishModel()
        {
            this.FishName = string.Empty;
            this.ZoneName = string.Empty;
            this.RodName = string.Empty;
            this.BaitName = string.Empty;
            this.ID1 = 0;
            this.ID2 = 0;
            this.ID3 = 0;
            this.ID4 = 0;
            this.Critical = false;
            this.FishCount = 0;
            this.FishType = FishDBFishTypeKind.Unknown;
            this.Result = FishResultStatusKind.NoBite;
            this.EarthTime = DateTime.Today;
            this.VanaTime = string.Empty;
            this.VanaWeekDay = Weekday.Unknown;
            this.MoonPhase = FFACETools.MoonPhase.Unknown;
            this.X = 0.0f;
            this.Y = 0.0f;
            this.Z = 0.0f;
            this.H = 0.0f;
        }
    }
    public class FishHistoryDBSummaryModel
    {
        public int Count { get; set; }
        public List<FishHistoryDBSummaryResultModel> Results { get; set; }
        public FishHistoryDBSummaryModel()
        {
            this.Count = 0;
            this.Results = new List<FishHistoryDBSummaryResultModel>();
            foreach (FishResultStatusKind v in Enum.GetValues(typeof(FishResultStatusKind)))
            {
                if (v != FishResultStatusKind.Unknown)
                {
                    FishHistoryDBSummaryResultModel result = new FishHistoryDBSummaryResultModel();
                    result.Result = v;
                    Results.Add(result);
                }
            }
        }
        public void Add(FishHistoryDBFishModel iFish)
        {
            this.Count += 1;
            foreach (FishHistoryDBSummaryResultModel v in this.Results)
            {
                if (v.Result == iFish.Result)
                {
                    v.Add(iFish);
                }
            }
            foreach (FishHistoryDBSummaryResultModel result in this.Results)
            {
                if (this.Count != 0) result.TotalPercent = (int)Math.Round(((double)result.Count / (double)this.Count) * 100d);
                else result.TotalPercent = 0;
                foreach (FishHistoryDBSummaryFishModel fish in result.Fishes)
                {
                    if (result.Count != 0) fish.Percent = (int)Math.Round(((double)fish.Count / (double)result.Count) * 100d);
                    else result.Percent = 0;
                    if (this.Count != 0) fish.TotalPercent = (int)Math.Round(((double)fish.Count / (double)this.Count) * 100d);
                    else result.TotalPercent = 0;
                }
            }
                        
        }
    }
    public class FishHistoryDBSummaryResultModel
    {
        public FishResultStatusKind Result { get; set; }
        public int Count { get; set; }
        public int Percent { get; set; }
        public int TotalPercent { get; set; }
        public List<FishHistoryDBSummaryFishModel> Fishes { get; set; }
        public FishHistoryDBSummaryResultModel()
        {
            this.Result = FishResultStatusKind.Unknown;
            this.Count = 0;
            this.Percent = 0;
            this.TotalPercent = 0;
            this.Fishes = new List<FishHistoryDBSummaryFishModel>();
        }
        public void Add(FishHistoryDBFishModel iFish)
        {
            this.Count += 1;

            bool foundFlg = false;
            foreach (FishHistoryDBSummaryFishModel fish in this.Fishes)
            {
                if (fish.FishName == iFish.FishName)
                {
                    foundFlg = true;
                    fish.Add(iFish);
                    break;
                }
            }
            if (!foundFlg)
            {
                FishHistoryDBSummaryFishModel fish = new FishHistoryDBSummaryFishModel(iFish);
                fish.FishName = iFish.FishName;
                fish.FishType = iFish.FishType;
                this.Fishes.Add(fish);
            }
            this.Fishes.Sort(FishHistoryDBSummaryFishModel.SortTypeName);
        }
    }
    public class FishHistoryDBSummaryFishModel
    {
        public string FishName { get; set; }
        public FishDBFishTypeKind FishType { get; set; }
        public int Count { get; set; }
        public int Percent { get; set; }
        public int TotalPercent { get; set; }
        public FishHistoryDBSummaryFishModel(FishHistoryDBFishModel iFish)
        {
            this.FishName = iFish.FishName;
            this.FishType = iFish.FishType;
            this.Count = 1;
            this.Percent = 0;
            this.TotalPercent = 0;
        }
        public void Add(FishHistoryDBFishModel iFish)
        {
            this.Count += 1;
        }
        public static int SortTypeName(FishHistoryDBSummaryFishModel iFish1, FishHistoryDBSummaryFishModel iFish2)
        {
            //1番目のキー：FishTypeでソート
            if (iFish1.FishType > iFish2.FishType)
            {
                return 1;
            }
            else if (iFish1.FishType < iFish2.FishType)
            {
                return -1;
            }
            else
            {
                //2番目のキー：FishNameでソート
                return string.Compare(iFish1.FishName, iFish2.FishName);
            }
        }

    }
}