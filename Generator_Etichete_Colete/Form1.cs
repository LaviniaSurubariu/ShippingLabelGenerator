using BarcodeLib;
using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Configuration;
using System.Drawing.Printing;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using Spire.Pdf;

namespace Generator_Etichete_Colete
{

    public partial class Form1 : Form
    {
        private int marimeMargine = 2;
        int verificareNr = 0;

        public Form1()
        {
            InitializeComponent();
            ingheataMenu();
            lbCodIdentificare.Visible= false;
            tbIdComanda.Visible= false;

        }

     



        //Calculeaza AWB-ul
        private string CalculareAWB()
        {
            verificareNr++;
            if (verificareNr > 9) verificareNr = 0;


            String AWB = "";

            AWB += tbNumeExpeditor.Text.Length.ToString()[0].ToString().ToString();
            AWB += verificareNr.ToString();
            AWB += tbNumeDestinatar.Text.Length.ToString()[0].ToString().ToString();
            AWB += DateTime.Now.TimeOfDay.Minutes.ToString();
            int milis = DateTime.Now.TimeOfDay.Milliseconds;
            while (milis.ToString().Length < 3)
            {
                milis = DateTime.Now.TimeOfDay.Milliseconds;
            }
            AWB += milis.ToString();
            AWB += DateTime.Now.TimeOfDay.Hours.ToString();
            AWB += DateTime.Now.Second.ToString();
            // textBox1.Text= AWB;
            return AWB;

        }




        private void Form1_Resize(object sender, EventArgs e)
        {
            AjustareForma();

        }
        private void AjustareForma()
        {
            switch (this.WindowState)
            {
                case FormWindowState.Maximized:
                    this.Padding = new Padding(0, 8, 8, 0);
                    break;

                case FormWindowState.Normal:
                    if (this.Padding.Top != marimeMargine)
                        this.Padding = new Padding(marimeMargine);
                    break;
            }
        }


        private void iconButtonMenu_Click(object sender, EventArgs e)
        {
            ingheataMenu();

        }
        private void ingheataMenu()
        { //ingheata meniul
            if (this.panelMenu.Width > 200)
            {
                panelMenu.Width = 150;
                pictureBox1.Visible = false;
                iconButtonMenu.Dock = DockStyle.Top;
                foreach (Button buton in panelMenu.Controls.OfType<Button>())
                {
                    buton.Text = " ";
                    buton.ImageAlign = ContentAlignment.MiddleCenter;
                    buton.Padding = new Padding(40, 0, 0, 0);

                }

            }
            else
            {//deschide meniul
                panelMenu.Width = 237;
                pictureBox1.Visible = true;
                iconButtonMenu.Dock = DockStyle.None;
                foreach (Button buton in panelMenu.Controls.OfType<Button>())
                {
                    buton.Text = buton.Tag.ToString();
                    buton.ImageAlign = ContentAlignment.MiddleCenter;
                    buton.Padding = new Padding(40, 0, 0, 0);

                }

            }
        }



        //calculeaza tariful total
        private void button1_Click(object sender, EventArgs e)
        {

            CalculeazaTotal();


        }


        //metoda calculeaza tarif total
        private void CalculeazaTotal()
        {
            errorProvider1.Clear();
            errorProvider2.Clear();

            double suma = 0;
            //daca greutatea e 0 sau nimic arunca erroare
            if (tbGreutate.Text == "" || Convert.ToDouble(tbGreutate.Text) == 0)
            {
                errorProvider1.SetError(tbGreutate, "Introduceti greutatea");
            }
            else
            {
                //verifica pentru fiecare greutate si adauga la suma
                if (Convert.ToDouble(tbGreutate.Text) < 1) suma += 10;
                else if (Convert.ToDouble(tbGreutate.Text) < 5) suma += 20;
                else if (Convert.ToDouble(tbGreutate.Text) < 10) suma += Convert.ToDouble(tbGreutate.Text) * 5;
                else if (Convert.ToDouble(tbGreutate.Text) >= 20.0001) errorProvider1.SetError(tbGreutate, 
                                                                        "Greutatea este prea mare. Va rugam introduceti o valoare sub 20.");
                else if (Convert.ToDouble(tbGreutate.Text) <= 20 || Convert.ToDouble(tbGreutate.Text) >= 10) suma += Convert.ToDouble(tbGreutate.Text) * 10;
                if (Convert.ToDouble(tbGreutate.Text) <= 20)
                {
                    //verifica checkboxurile si adauga la suma
                    if (cbFragil.Checked) suma += 5;
                    if (cbVoluminos.Checked) suma += 5;
                    if (cbPersonal.Checked) suma += 5;
                    if (cbRidicareSambata.Checked) suma += 5;
                    if (cbConfirmarePrimire.Checked) suma += 5;
                    if (cbLivrareDomiciliu.Checked) suma += 5;
                }

            }

            tbTarif.Text = suma.ToString();
        }

        //Verificare pt greutate
        private void tbGreutate_KeyPress(object sender, KeyPressEventArgs e)
        {

            //verifica sa nu fie litera
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            //verifica sa fie .
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

        }


        //generare AWB
        private void btnGenereazaAWB_Click(object sender, EventArgs e)
        {
            errorProvider2.Clear();
            errorProvider1.Clear();

            //expeditor
            if (tbNumeExpeditor.Text == "")
            {
                errorProvider2.Clear();
                pictureBox2.Image = null;
                errorProvider2.SetError(tbNumeExpeditor, "Introduceti numele/denumirea firmei");

            }

            else if (cbJudetExpeditor.Text == "")
            {
                errorProvider2.Clear();
                pictureBox2.Image = null;
                errorProvider2.SetError(cbJudetExpeditor, "Introduceti judetul");

            }
            else if (tbLocalitateExpeditor.Text == "")
            {
                errorProvider2.Clear();
                pictureBox2.Image = null;
                errorProvider2.SetError(tbLocalitateExpeditor, "Introduceti localitatea");

            }
            else if (tbAdresaExpeditor.Text == "")
            {
                errorProvider2.Clear();
                pictureBox2.Image = null;
                errorProvider2.SetError(tbAdresaExpeditor, "Introduceti adresa");

            }
            else if (tbCodPostalExpeditor.Text == "")
            {
                errorProvider2.Clear();
                pictureBox2.Image = null;
                errorProvider2.SetError(tbCodPostalExpeditor, "Introduceti codul postal");

            }
            else if (tbTelefonExpeditor.Text == "")
            {
                errorProvider2.Clear();
                pictureBox2.Image = null;
                errorProvider2.SetError(tbTelefonExpeditor, "Introduceti numarul de telefon");

            }
            else if (tbEmailExpeditor.Text == "")
            {
                errorProvider2.Clear();
                pictureBox2.Image = null;
                errorProvider2.SetError(tbEmailExpeditor, "Introduceti email");

            }

            //destinatar
            else if (tbNumeDestinatar.Text == "")
            {
                errorProvider2.Clear();
                pictureBox2.Image = null;
                errorProvider2.SetError(tbNumeDestinatar, "Introduceti numele/denumirea firmei");

            }
            else if (cbJudetDestinatar.Text == "")
            {
                errorProvider2.Clear();
                pictureBox2.Image = null;
                errorProvider2.SetError(cbJudetDestinatar, "Introduceti judetul");

            }
            else if (tbLocalitateDestinatar.Text == "")
            {
                errorProvider2.Clear();
                pictureBox2.Image = null;
                errorProvider2.SetError(tbLocalitateDestinatar, "Introduceti localitatea");

            }
            else if (tbAdresaDesinatar.Text == "")
            {
                errorProvider2.Clear();
                pictureBox2.Image = null;
                errorProvider2.SetError(tbAdresaDesinatar, "Introduceti adresa");

            }
            else if (tbCodPostalDestinatar.Text == "")
            {
                errorProvider2.Clear();
                pictureBox2.Image = null;
                errorProvider2.SetError(tbCodPostalDestinatar, "Introduceti codul postal");

            }
            else if (tbTelefonDestinatar.Text == "")
            {
                errorProvider2.Clear();
                pictureBox2.Image = null;
                errorProvider2.SetError(tbTelefonDestinatar, "Introduceti numarul de telefon");

            }
            else if (tbEmailDestinatar.Text == "")
            {
                errorProvider2.Clear();
                pictureBox2.Image = null;
                errorProvider2.SetError(tbEmailDestinatar, "Introduceti email");

            }
            else if (tbTarif.Text == "" || Convert.ToInt32(tbTarif.Text) == 0)
            {
                errorProvider2.Clear();
                pictureBox2.Image = null;
                errorProvider1.SetError(tbTarif, "Calculati total");
            }

            else if (pictureBox2.Image != null)
            {
                MessageBox.Show("AWB deja generat!");
            }

            else
            {
                errorProvider2.Clear();
                CalculeazaTotal();

                Barcode barcode = new Barcode();
                Color foreColore = Color.Black;
                Color backColore = Color.Transparent;
                string AWB = CalculareAWB();
                Image img = barcode.Encode(TYPE.UPCA, AWB, foreColore, backColore,
                    (int)(pictureBox2.Width * 1), (int)(pictureBox2.Height * 1));
                pictureBox2.Image = img;
                lbCodIdentificare.Visible = true;
                tbIdComanda.Visible = true;
                tbIdComanda.Text = AWB;
                


            }

        }




        //stergere date
        private void stergeDate()
        {
            tbIdComanda.Clear();
            tbIdComanda.Visible=false;
            lbCodIdentificare.Visible=false;
            tbNumeExpeditor.Clear();
            cbJudetExpeditor.ResetText();
            tbLocalitateExpeditor.Clear();
            tbAdresaExpeditor.Clear();
            tbCodPostalExpeditor.Clear();
            tbTelefonExpeditor.Clear();
            tbEmailExpeditor.Clear();

            tbNumeDestinatar.Clear();
            cbJudetDestinatar.ResetText();
            tbLocalitateDestinatar.Clear();
            tbAdresaDesinatar.Clear();
            tbCodPostalDestinatar.Clear();
            tbTelefonDestinatar.Clear();
            tbEmailDestinatar.Clear();

            tbGreutate.Clear();
            cbConfirmarePrimire.Checked = false;
            cbFragil.Checked = false;
            cbVoluminos.Checked = false;
            cbPersonal.Checked = false;
            cbRidicareSambata.Checked = false;
            cbLivrareDomiciliu.Checked = false;
            tbDetalii.Clear();
            tbTarif.Clear();
            errorProvider1.Clear();
            errorProvider2.Clear();
            pictureBox2.Image = null;
        }
        private void iconButton1_Click(object sender, EventArgs e)
        {
            stergeDate();


        }



        //printare
        private void iconButton2_Click(object sender, EventArgs e)
        {


            if (pictureBox2.Image == null)
            {
                MessageBox.Show("Va rugam introduceti datele.","Eroare");
                return;
            }

            else
            {
                btnGenereazaAWB.Visible = false;
                label35.Visible = false;
                button1.Visible = false;
                string titlu = groupBox1.Text.ToString();
                groupBox1.Text = "";
                save(panelEcran, "poza.bmp");
                btnGenereazaAWB.Visible = true;
                label35.Visible = true;
                button1.Visible = true;
                groupBox1.Text = titlu;




                MessageBox.Show("AWB printat!");
                stergeDate();

            }
            


        }


        //metoda salvare
        private void save(Control c, string fisier)
        {
            Bitmap img = new Bitmap(c.Width, c.Height);
            c.DrawToBitmap(img, new Rectangle(c.ClientRectangle.X,
                c.ClientRectangle.Y,
                c.ClientRectangle.Width,
                c.ClientRectangle.Height));
            //c.DrawToBitmap(img);
           img.Save(fisier);
            img.Dispose();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
