using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoveAllSub
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (string file in Directory.EnumerateFiles(@"D:\home\Google Drive\Google Fotos", "*.*", SearchOption.AllDirectories))
            {
                Console.WriteLine("Processing "+ file);
                String extension = Path.GetExtension(file);
                DateTime time = File.GetCreationTime(file);
                if (isImage(file))
                {
                    try
                    {
                        using (Image myImage = Image.FromFile(file))
                        {
                            time = DateTaken(myImage);
                            if (time == new DateTime(0))
                            {
                                throw new Exception("");
                            }
                        }
                    } catch
                    {
                        time = File.GetCreationTime(file);
                    }
                }
                String path = @"D:\home\Google Drive\Google Fotos\reorganize\"+extension.Replace(".","")+@"\" + time.ToString(@"yyyy\\MM")+@"\";
                String newFile;
                int i = -1;
                do
                {
                    i++;
                    if (i == 0)
                    {
                        newFile = path + time.ToString(@"dd-HHmmss") + extension;
                    } else
                    {
                        path = @"C:\duplicated\"+ extension.Replace(".", "") + @"\" + time.ToString(@"yyyy\\MM") + @"\";
                        newFile = path + time.ToString(@"dd-HHmmss")+"_"+i + extension;
                    }
                } while (File.Exists(newFile));
                try
                {
                    Directory.CreateDirectory(path);
                    File.Move(file, newFile);
                }
                catch (Exception ex){
                }
                Console.WriteLine("Did " + file);
            }
            Console.ReadKey();
        }

        public static DateTime DateTaken(Image getImage)
        {
            int DateTakenValue = 0x9003; //36867;

            if (!getImage.PropertyIdList.Contains(DateTakenValue))
                return new DateTime(0);

            string dateTakenTag = System.Text.Encoding.ASCII.GetString(getImage.GetPropertyItem(DateTakenValue).Value);
            string[] parts = dateTakenTag.Split(':', ' ');
            int year = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int day = int.Parse(parts[2]);
            int hour = int.Parse(parts[3]);
            int minute = int.Parse(parts[4]);
            int second = int.Parse(parts[5]);

            return new DateTime(year, month, day, hour, minute, second);
        }

        public static bool isImage(string file)
        {
            try
            {
                using (Image imgInput = Image.FromFile(file)) {
                    using (Graphics gInput = Graphics.FromImage(imgInput))
                    {
                        ImageFormat thisFormat = imgInput.RawFormat;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }
    }    
}
