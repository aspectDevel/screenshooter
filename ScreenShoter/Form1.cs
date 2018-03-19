using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScreenShoter
{
    public partial class Form1 : Form
    {
        private string path;
        private int counter = 0;
        private int interval;
        private bool working = false;
        Timer timer;

        public Form1()
        {
            InitializeComponent();
        }

        /**
         * Событие при нажатии на "Выбрать папку"
         */
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                path = fbd.SelectedPath;
                button2.Enabled = true;
            }
            fbd.Dispose();
            
        }

        /**
         * Событие при нажатии на "Запустить/Остановить"
         */
        private void button2_Click(object sender, EventArgs e)
        {
            /**
             * Проверка что интервал между снимками не менее одной секунды
             **/
            interval = Convert.ToInt32(numericUpDown1.Value);
            if(interval < 1)
            {
                MessageBox.Show("Указан некорректный интервал");
                return;
            }

            /**
             * Если выключен
             */
            if(!working)
            {
                /**
                 * Включим
                 */
                on();
            }
            /**
             * Если включен
             */
            else
            {
                /**
                 * Выключим
                 */
                off();
            }

        }

        /**
         * Включение
         */
        private void on()
        {
            numericUpDown1.Enabled = false;
            button1.Enabled = false;
            button2.Text = "Остановить";

            timer = new Timer();
            timer.Interval = interval * 1000;
            timer.Tick += new EventHandler(doScreenSave);
            timer.Start();
            working = true;
        }


        /**
         * Выключение
         */
        private void off()
        {
            numericUpDown1.Enabled = true;
            button1.Enabled = true;
            button2.Text = "Запустить";

            timer.Stop();
            timer.Dispose();

            working = false;
        }

        /**
         * Сам скриншот
         */
        private void doScreenSave(object a, EventArgs b)
        {
            using (var bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height))
            {
                using (Graphics img = Graphics.FromImage(bmp))
                {
                    img.CopyFromScreen(0, 0, 0, 0, bmp.Size);
                    try
                    {
                        bmp.Save(path + "\\" + ++counter + ".png");
                    }
                    catch(Exception e)
                    {
                        off();
                        MessageBox.Show(e.Message, "Ошибка"); // TODO: вычленить ошибку записи из-за не хватки прав.
                    }
                    

                }
                    
            }
                
        }
    }
}
