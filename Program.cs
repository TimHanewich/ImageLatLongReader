using System;
using ExifLib;
using System.IO;
using Newtonsoft.Json;

namespace testing
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Path to your file?");
                    string path = Console.ReadLine();
                    path = path.Replace("\"", "");
                    Stream s = System.IO.File.OpenRead(path);
                    ExifReader er = new ExifReader(s);
                    
                    double[] lats;
                    er.GetTagValue(ExifTags.GPSLatitude, out lats);

                    double[] lons;
                    er.GetTagValue(ExifTags.GPSLongitude, out lons);

                    //Convert
                    float latitude = DMS_to_Decimal(lats);
                    float longitude = DMS_to_Decimal(lons);


                    //Invert them if it is S or W
                    string lat_ref;
                    er.GetTagValue(ExifTags.GPSLatitudeRef, out lat_ref);
                    if (lat_ref == "S")
                    {
                        latitude = latitude * -1;
                    }
                    string lon_ref;
                    er.GetTagValue(ExifTags.GPSLongitudeRef, out lon_ref);
                    if (lon_ref == "W")
                    {
                        longitude = longitude * -1;
                    }

                    Console.WriteLine(latitude.ToString() + "," + longitude.ToString());
                    Console.WriteLine();
                }
                catch
                {
                    Console.WriteLine("Fatal failure!");
                }
                
            }
        }

        public static float DMS_to_Decimal(double[] vals)
        {
            float ToReturn = 0;
            ToReturn = (float)vals[0];
            ToReturn = ToReturn + (float)(vals[1]/60);
            ToReturn = ToReturn + (float)(vals[2]/3600);
            return ToReturn;
        }
    }
}
