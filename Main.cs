/*
 * Created by SharpDevelop.
 * User: Yangxin
 * Date: 2014/8/16
 * Time: 1:14
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;

using System.Text;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

using System.Threading;
using DataAccess.Structural.Pkpm;
using DataAccess.Structural.Pkpm.Converter;
using DataAccess.Structural.Pkpm.Models;

using Warrentech.Velo.BimDataModel;
//using Warrentech.Velo.CodeCheck;


using System.Data.OleDb;

using System.Data; 

namespace test
{
    public class beamEl
    {
        public double posX0, posY0, posX1, posY1;
        public int b, h;
        public int think;
        public int Ev1, Ev2;
        //public XYZ st, ed;
        //public 
        public beamEl(double posX0, double posY0, double posX1, double posY1)
        {
            this.posX0 = posX0;
            this.posY0 = posY0;
            this.posX1 = posX1;
            this.posY1 = posY1;
            //st = new XYZ(posX0, posY0, 0);
            //ed = new XYZ(posX1, posY1, 0);
        }
        public void setSize(int b, int h)
        {
            this.b = b;
            this.h = h;
        }
        public void setSize(int think)
        {
            this.think = think;
        }
        public string getK()
        {
            if (Math.Round(posY1, 0) - Math.Round(posY0, 0) <= 5) return "inf";
            if (Math.Round(posX1, 0) - Math.Round(posX0, 0) <= 5) return "0";
            return (Math.Abs((Math.Round(posX1, 0) - Math.Round(posX0, 0)) / (Math.Round(posY1, 0) - Math.Round(posY0, 0)))).ToString();
        }
    }
    class point
    {
        public int x, y;
    }
    class line
    {
        public double x1, y1, x2, y2;
        public line(double x1, double y1, double x2, double y2)
        {
            this.x1 = x1; this.x2 = x2; this.y1 = y1; this.y2 = y2;
        }
    }
    class Area
    {
        public List<line> lines;

    }
	class MainClass
	{
        public static string filepath2 = @"D:\杨新\接收文件\保利横琴-0707弹塑性模型\";
        public static string filepath = @"D:\杨新\pktest\";
        public static bool equ(beamEl el1, beamEl el2)
        {
            if (el1.getK() == el2.getK()
                //&&
                //el1.b == el2.b &&
                //el1.h == el2.h //&&
                //el1.Ev1 == 0 &&
                //el1.Ev2 == 0 &&
                //el2.Ev1 == 0 &&
                //el2.Ev2 == 0
                )
                return true;
            else return false;
        }
        public static bool eq(double d1, double d2)
        {
            if (Math.Round(d1, 0) == Math.Round(d2, 0)) return true;
            else return false;
        }
        public static List<beamEl> guibing(List<beamEl> list)
        {
            List<beamEl> newlist = new List<beamEl>();
            newlist.Add(list[0]); list.RemoveAt(0);

            foreach (beamEl l in list)
            {
                int get = 0;
                for (int i = 0; i < newlist.Count; i++)
                {
                    beamEl newL = newlist[i];
                    if (
                        l.posX0 == newL.posX0 && l.posY0 == newL.posY0
                        &&
                        equ(newL, l)
                        )
                    {
                        newlist[i].posX0 = l.posX1;
                        newlist[i].posY0 = l.posY1;
                        get = 1;
                        break;
                    }
                    else if (
                        l.posX1 == newL.posX1 && l.posY1 == newL.posY1
                        &&
                        equ(newL, l)
                        )
                    {
                        newlist[i].posX1 = l.posX0;
                        newlist[i].posY1 = l.posY0;
                        get = 1;
                        break;
                    }
                    else if (
                        eq(l.posX1, newL.posX0) && eq(l.posY1, newL.posY0)
                        &&
                        equ(newL, l)
                        )
                    {
                        newlist[i].posX0 = l.posX0;
                        newlist[i].posY0 = l.posY0;
                        get = 1;
                        break;
                    }
                    else if (
                        eq(l.posX0, newL.posX1) && eq(l.posY0, newL.posY1)
                        &&
                        equ(newL, l)
                        )
                    {
                        newlist[i].posX1 = l.posX1;
                        newlist[i].posY1 = l.posY1;
                        if (equ(newL, l) && l.getK() == "inf" && newlist[i].getK() == "inf" && (Math.Round(l.posY0, 0) == 6279 && Math.Round(newlist[i].posY0, 0) == 6279))
                        Console.WriteLine("bbbbbb"+newlist[i].posX0 + ",\t" + newlist[i].posX1 + ",\t" + newlist[i].posY0 + ",\t" + newlist[i].posY1);
                        get = 1;
                        break;
                    }
                    if (equ(newL, l) && l.getK() == "inf" && newlist[i].getK() == "inf" && (Math.Round(l.posY0, 0) == 6279 && Math.Round(newlist[i].posY0, 0) == 6279))
                    {
                        Console.WriteLine(l.posX0 + ",\t" + l.posX1 + ",\t" + l.posY0 + ",\t" + l.posY1);
                        Console.WriteLine(newlist[i].posX0 + ",\t" + newlist[i].posX1 + ",\t" + newlist[i].posY0 + ",\t" + newlist[i].posY1);
                        Console.WriteLine("======");
                    }

                }
                if (get == 0) newlist.Add(l);
            }

            return newlist;
        }
        public static DataTable ReadAllData(string tableName,  string mdbPath)   
        {
            bool success=false;
        DataTable dt = new DataTable();   
        try
        {
            DataRow dr;   
 
        //1、建立连接 C#操作Access之读取mdb  

            string strConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + mdbPath + ";";   
        OleDbConnection odcConnection = new OleDbConnection(strConn);   
 
        //2、打开连接 C#操作Access之读取mdb  
        odcConnection.Open();   
 
        //建立SQL查询   
        OleDbCommand odCommand = odcConnection.CreateCommand();   
 
        //3、输入查询语句 C#操作Access之读取mdb  

        string where = "";
        where = " where `FloorID`=7";
        odCommand.CommandText = "select * from " + tableName + where;
 
        //建立读取   
        OleDbDataReader odrReader = odCommand.ExecuteReader();   
 
        //查询并显示数据   
        int size = odrReader.FieldCount;   
        for (int i = 0; i < size; i++)   
        {   
            DataColumn dc;   
            dc = new DataColumn(odrReader.GetName(i));   
            dt.Columns.Add(dc);   
        }   
        while (odrReader.Read())   
        {   
            dr = dt.NewRow();   
            for (int i = 0; i < size; i++)   
            {   
        dr[odrReader.GetName(i)] =   
        odrReader[odrReader.GetName(i)].ToString();   
            }   
            dt.Rows.Add(dr);   
        }   
        //关闭连接 C#操作Access之读取mdb  
        odrReader.Close();   
        odcConnection.Close();   
        success = true;   
        return dt;   
            }   
            catch (Exception e)  
            {
        success = false;
        Console.WriteLine("fail:"+e.ToString());
        return dt;
            }
        }

        
		public static void Main(string[] args)
		{
            DataTable dt = new DataTable();
            bool ifDbSuccess = false;
            string location = filepath+"BeamReinS.MDB";
            dt = ReadAllData("BeamSegMap", location);
            Console.WriteLine(dt.Rows.Count);
            /*
            foreach (DataRow dr in dt.Rows)
            {
                if (dr.ItemArray[1].ToString() == "1122")
                Console.WriteLine(dr.ItemArray[1].ToString());
            }*/



            List<line> lineList = new List<line>();
            List<point> pointList = new List<point>();
            double pkpmX = 0;
            double pkpmY = 0;

            int xx = 0;
            int yy = 0;
            beamEl bl1 = new beamEl(1, 5, 3, 5);
            beamEl bl2 = new beamEl(1, 5, 3, 5);
            if(bl1.getK() == bl2.getK())
            Console.WriteLine("======ok==========");



			//初始化
            PkpmHelper.PkpmWorkingDirectory = new DirectoryInfo(filepath);
            PkpmHelper.JwsFileName = filepath + "1.JWS";//保利横琴-飞哥新
            
            //way1
            PmModel model = new PmModel();
            ManualResetEvent aa =new ManualResetEvent(true);
            ReadSatFileThread pk11read = new ReadSatFileThread(model, aa);
            StringBuilder errorMessage = new StringBuilder();
            pk11read.ReadSatFileRun(errorMessage);


            Console.WriteLine(errorMessage);


            Console.WriteLine(model.StoreysNL[1].WallColumnData.Count);
            foreach (KeyValuePair<int, PmElementData> kvp in model.StoreysNL[1].WallColumnData)
            {
                Console.WriteLine(kvp.Value.StringProperties.Count);
                foreach (KeyValuePair<string, string> result in kvp.Value.StringProperties)
                {
                    //if(result.Key.IndexOf("V")>=0)
                    Console.WriteLine(result.Key.ToString() + ":" + result.Value.ToString());
                }
                foreach (KeyValuePair<string, double> result in kvp.Value.DoubleProperties)
                {
                    //if(result.Key.IndexOf("V")>=0)
                    Console.WriteLine(result.Key.ToString()+":"+result.Value.ToString());
                }
                //Console.WriteLine("==="+kvp.Value.MatrixProperties.Count);
                //break;
                foreach (KeyValuePair<string, Dictionary<string, double>> result in kvp.Value.DoubleArrayProperties)
                {
                    //Console.WriteLine(result.Key.ToString());
                } 
            }
            


            try
            {
                //way2
                BimProject bimProject = new BimProject();
                PKPMGeometryInfoLoader pkpmLoader = new PKPMGeometryInfoLoader(bimProject);
                pkpmLoader.LoadGeometryInfo();

                PKPMReinforcementLoader pkpmReinLoader = new PKPMReinforcementLoader(bimProject);
                StringBuilder readBinaryMessage = new StringBuilder();
                StringBuilder loadReinMessage = new StringBuilder();
                pkpmReinLoader.LoadReinforcement( readBinaryMessage,  loadReinMessage);
                Console.WriteLine(readBinaryMessage +","+ loadReinMessage);

                StoreyNumber storeyNum = new StoreyNumber(1);
                List<beamEl> blm = new List<beamEl>();
                //Console.WriteLine(model.StoreySdata[1].BeamData[2].ElementType);
                //Console.WriteLine(model.StoreySdata[1].BeamData[2].GetType());
                //model.StoreysPJ[1].BeamData.
                Console.WriteLine("=========GetBeamElementList=============");


                foreach (KeyValuePair<BeamLayoutElement, BeamElement> kvp in bimProject.Buildings[0].Storeys[storeyNum].GetBeamMap())
                {
                    Console.Write(kvp.Key.ID.ToString());
                    Console.Write(kvp.Key.Profile.ProfileType.ToString());
                    Console.WriteLine(kvp.Key.Profile.GetProfileString());
                    //Console.WriteLine(kvp.Value.);
                }



                foreach (BeamElement kvp in bimProject.Buildings[0].Storeys[storeyNum].GetBeamElementList())
                {

                    //Console.WriteLine(kvp.ProfileType.ToString()+kvp.ID.ToString()+kvp.BeamLayoutElement.Profile.Name);

                    //if(kvp.BeamResult)
                    //Console.WriteLine(kvp.BeamResult.UID+" / "+kvp.BeamResult.EndMoment);
                    //Console.WriteLine(kvp.BeamResult.Asu1 + "/" + kvp.BeamResult.Asu2 + "/ " + kvp.BeamResult.Asu3);
                   // Console.WriteLine(kvp.BeamResult.Asd1+"/"+kvp.BeamResult.Asd2+"/ "+kvp.BeamResult.Asd3);
                    //Console.WriteLine(kvp.BeamLayoutElement.Uid);

                }
                Console.WriteLine("=========GetAllColumnElement=============");
                foreach (ColumnElement bl in bimProject.Buildings[0].Storeys[storeyNum].GetAllColumnElement())
                {
                    

                    //string beamType = bl.ColumnLayoutElement.Profile.GetProfileString();
                    //Regex PatternText = new Regex(@"工字形截面");
                    //var resultText = PatternText.Match(beamType).Groups;

                    //if (resultText[1].Length > 0)
                    //Console.WriteLine("\n"+bl.ColumnLayoutElement.Profile.ProfileType.ToString() + "," + bl.ColumnLayoutElement.Profile.GetProfileLength() + bl.ColumnLayoutElement.Profile.GetProfileWidth());
                    //Console.WriteLine("轴压比UC="+bl.ColumnResult.Uc);
                    //Console.WriteLine("" + bl.GetGFCInputFile());
                    //Console.WriteLine(bl.ColumnLayoutElement.Node.TopHeight);

                    //Console.WriteLine(bl.BeamLayoutElement.Profile.GetDiameter()+","+bl.BeamLayoutElement.Profile.GetProfileWidth() + ":  " + bl.BeamLayoutElement.Profile.Name);
                    
                    /*
                    if (pkpmX == 0)
                    {
                        pkpmX = bl.BeamLayoutElement.GetAnchorPoint1().X;
                        pkpmY = bl.BeamLayoutElement.GetAnchorPoint1().Y;
                    }
                    blm.Add(new beamEl(bl.BeamLayoutElement.GetAnchorPoint1().X - pkpmX , (bl.BeamLayoutElement.GetAnchorPoint1().Y - pkpmY),(bl.BeamLayoutElement.GetAnchorPoint2().X - pkpmX) ,(bl.BeamLayoutElement.GetAnchorPoint2().Y - pkpmY)));
                
                     */
                }
                //blm = guibing(blm);

                Console.ReadKey();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
		}
	}


}
