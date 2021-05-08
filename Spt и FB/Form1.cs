using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using EPPlus;
namespace Spt_и_FB
{
    public partial class Form1 : Form
    {
        static string path = Path.GetDirectoryName(Application.StartupPath)+ "excel.xlsx";
        public static int Current_Column_FB = 2;
        public static int Current_Column_SPT = 2;
        public static FileInfo file = new FileInfo(@path);
        ExcelPackage package = new ExcelPackage(@file);
        public Form1()
        {
            
            InitializeComponent();
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(@path);
            }
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("FB");
            ExcelWorksheet worksheet2 = package.Workbook.Worksheets.Add("SPT");
            for (int i = 1; i < 9; i++)
            {
                worksheet.Cells[i, 1].Style.WrapText = true;
                worksheet2.Cells[i, 1].Style.WrapText = true;
            }
            worksheet.Cells["A1"].Value = "Количество заявок = ";
            worksheet.Cells["A2"].Value = "Вероятность прихода заявки = ";
            worksheet.Cells["A3"].Value = "Длина короткой заявки = ";
            worksheet.Cells["A4"].Value = "Максимальная длина = ";
            worksheet.Cells["A5"].Value = "Длительность процессорного кванта = ";
            worksheet.Cells["A6"].Value = "Результаты: \r \nСумма длин всех заявок";
            worksheet.Cells["A7"].Value = "Количество тактов = ";
            worksheet.Cells["A8"].Value = "Время ожидания короткой заявки = ";

            worksheet2.Cells["A1"].Value = "Количество заявок = ";
            worksheet2.Cells["A2"].Value = "Вероятность прихода заявки = ";
            worksheet2.Cells["A3"].Value = "Длина короткой заявки = ";
            worksheet2.Cells["A4"].Value = "Максимальная длина = ";
            worksheet2.Cells["A5"].Value = "Длительность процессорного кванта = ";
            worksheet2.Cells["A6"].Value = "Результаты: \r \nСумма длин всех заявок";
            worksheet2.Cells["A7"].Value = "Количество тактов = ";
            worksheet2.Cells["A8"].Value = "Время ожидания короткой заявки = ";

        }
        private void Form1_Load(object sender, EventArgs e)
        {
        
        }
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();//FB
            Queue<int> queue = new Queue<int>();
            int kol = (int)numericUpDown4.Value; // Количестово заявок
            int ML = (int)numericUpDown5.Value; //максимальная длина заявки
            double R = (double)numericUpDown1.Value; //вероятность прихода ненулевой заявки
            int LK = (int)numericUpDown3.Value; // длина короткой заявки
            int L = (int)numericUpDown2.Value; // длительность рещения задачи
            int[] mas = new int[kol];
            int count_short = 0, tact_count = 0, short_time = 0, null_request = 0;
            Random rnd = new Random();
            int Sum = 0;
            for (int j = 0; j < kol; j++)
            {
                if (rnd.Next(100) < R)
                {
                    int temp = rnd.Next(ML);
                    queue.Enqueue(temp);
                    Sum += temp;
                }
                else
                    queue.Enqueue(0);
            }
            while(queue.Count != 0)
            {
                if (queue.Peek() != 0)
                {
                    if(queue.Peek()<= LK)
                    {
                        count_short++;
                        tact_count++;
                        short_time += L - queue.Dequeue();
                    }
                    else
                    {
                        tact_count++;
                        int temp = queue.Dequeue() - L;
                        if (temp > 0)
                            queue.Enqueue(temp);
                    }
                }
                else 
                {
                    queue.Dequeue();
                    null_request++;
                }
            }

            float average = (float)short_time / (float)count_short;
            textBox1.Text += '\n' + "Сумма длин всех заявок в очереди = " + Convert.ToString(Sum) + "\r";
            textBox1.Text += '\n' + "Количество выполненных тактов = " +
            Convert.ToString(tact_count) + "\n \r";
            textBox1.Text += '\n' + "Среднее время ожидания одной короткой заявки = " + Convert.ToString(average) + " ";
            //ExcelPackage package = new ExcelPackage(file);
            ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
            //worksheet.Cells["A1"].Value = "Сумма длин всех заявок в очереди = ";
            worksheet.Cells[1, Current_Column_FB].Value = Convert.ToString(kol);
            worksheet.Cells[2, Current_Column_FB].Value = Convert.ToString(R);
            worksheet.Cells[3, Current_Column_FB].Value = Convert.ToString(LK);
            worksheet.Cells[4, Current_Column_FB].Value = Convert.ToString(ML);
            worksheet.Cells[5, Current_Column_FB].Value = Convert.ToString(L);

            worksheet.Cells[6, Current_Column_FB].Value = Convert.ToString(Sum);
            worksheet.Cells[7, Current_Column_FB].Value = Convert.ToString(tact_count);
            worksheet.Cells[8, Current_Column_FB].Value = Convert.ToString(average);
            Current_Column_FB++;//переход к след. столбцу в excel
            package.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            int kol = (int)numericUpDown4.Value; // Количестово заявок
            int ML = (int)numericUpDown5.Value; //максимальная длина заявки
            double R = (double)numericUpDown1.Value; //вероятность прихода ненулевой заявки
            int LK = (int)numericUpDown3.Value; // длина короткой заявки
            int L = (int)numericUpDown2.Value; // длительность рещения задачи
            int[] mas = new int[kol];
            Random rnd = new Random();
            int count_short = 0, tact_count = 0, short_time = 0, null_request = 0;
            int Sum = 0;
            
            for (int j = 0; j < kol; j++)
            {
                if (rnd.Next(100) < R)
                    mas[j] = rnd.Next(ML);
                else
                    mas[j] = 0;
                Sum += mas[j];
            }
            Array.Sort(mas);
            for (int i = 0; i < kol; i++)
            {
                if (mas[i] > 0)
                {
                    if (mas[i] <= LK)
                    {
                        count_short++;
                        tact_count++;
                        short_time += L - mas[i];
                        mas[i] = 0;
                    }
                    if (mas[i] > LK)
                    {
                        int current = mas[i];
                        while (current >= 0)
                        {
                            current -= L;
                            tact_count++;
                        }
                        mas[i] = 0;
                    }
                }
                else
                {

                    null_request++;
                }
            }
            float average = (float)short_time / (float)count_short;
            textBox1.Text += '\n' + "Сумма длин всех заявок в очереди = " + Convert.ToString(Sum) + "\r";
            textBox1.Text += '\n' + "Количество выполненных тактов = " +
            Convert.ToString(tact_count) + "\n \r";
            textBox1.Text += '\n' + "Среднее время ожидания одной короткой заявки = " + Convert.ToString(average) + " ";
            // ExcelPackage package = new ExcelPackage(file);
            ExcelWorksheet worksheet = package.Workbook.Worksheets[2];
            //worksheet.Cells["A1"].Value = "Сумма длин всех заявок в очереди = ";
            worksheet.Cells[1, Current_Column_SPT].Value = Convert.ToString(kol);
            worksheet.Cells[2, Current_Column_SPT].Value = Convert.ToString(R);
            worksheet.Cells[3, Current_Column_SPT].Value = Convert.ToString(LK);
            worksheet.Cells[4, Current_Column_SPT].Value = Convert.ToString(ML);
            worksheet.Cells[5, Current_Column_SPT].Value = Convert.ToString(L);

            worksheet.Cells[6, Current_Column_SPT].Value = Convert.ToString(Sum);
            worksheet.Cells[7, Current_Column_SPT].Value = Convert.ToString(tact_count);
            worksheet.Cells[8, Current_Column_SPT].Value = Convert.ToString(average);
            Current_Column_SPT++;//переход к след. столбцу в excel
            package.Save();
        }
    } 
}