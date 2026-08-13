using System;
using System.Drawing;
using System.Windows.Forms;

namespace BilheteriaTeatro
{
    public partial class Form1 : Form
    {
        private const int total_fileiras = 15;
        private const int total_poltronas = 40;

        private int[,] poltronas;
        private Button[,] botoes;

        private decimal[] precos = {
            50.00m, 50.00m, 50.00m, 50.00m, 50.00m,
            30.00m, 30.00m, 30.00m, 30.00m, 30.00m,
            15.00m, 15.00m, 15.00m, 15.00m, 15.00m
        };

        private Panel painel;
        private Button btn_faturamento;
        private Label lbl_resultado;

        public Form1()
        {
            InitializeComponent();
            Inicializar();
        }

        private void Inicializar()
        {
            poltronas = new int[total_fileiras, total_poltronas];

            for (int i = 0; i < total_fileiras; i++)
                for (int j = 0; j < total_poltronas; j++)
                    poltronas[i, j] = 0;

            this.Text = "Bilheteria";
            this.Size = new Size(1100, 750);

            CriarInterface();
        }

        private void CriarInterface()
        {
            painel = new Panel();
            painel.Location = new Point(10, 10);
            painel.Size = new Size(1050, 620);
            painel.AutoScroll = true;
            painel.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(painel);

            CriarBotoes();

            btn_faturamento = new Button();
            btn_faturamento.Text = "Faturamento";
            btn_faturamento.Location = new Point(10, 650);
            btn_faturamento.Size = new Size(150, 35);
            btn_faturamento.Click += CalcularFaturamento;
            this.Controls.Add(btn_faturamento);

            lbl_resultado = new Label();
            lbl_resultado.Location = new Point(180, 650);
            lbl_resultado.Size = new Size(700, 35);
            lbl_resultado.Text = "Veja o faturamento";
            this.Controls.Add(lbl_resultado);
        }

        private void CriarBotoes()
        {
            botoes = new Button[total_fileiras, total_poltronas];
            int tamanho = 20;
            int espaco = 2;

            for (int i = 0; i < total_fileiras; i++)
            {
                for (int j = 0; j < total_poltronas; j++)
                {
                    Button btn = new Button();
                    btn.Size = new Size(tamanho, tamanho);
                    btn.Location = new Point(j * (tamanho + espaco) + 35, i * (tamanho + espaco) + 10);
                    btn.Tag = new Tuple<int, int>(i, j);
                    btn.BackColor = Color.White;
                    btn.Click += ClicarPoltrona;
                    painel.Controls.Add(btn);
                    botoes[i, j] = btn;
                }
            }

            for (int i = 0; i < total_fileiras; i++)
            {
                Label lbl = new Label();
                lbl.Text = (i + 1).ToString();
                lbl.Location = new Point(5, i * (tamanho + espaco) + 10);
                lbl.Size = new Size(28, 20);
                lbl.Font = new Font("Arial", 8, FontStyle.Bold);
                lbl.TextAlign = ContentAlignment.MiddleRight;
                painel.Controls.Add(lbl);
            }
        }
        

        private void ClicarPoltrona(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Tuple<int, int> pos = (Tuple<int, int>)btn.Tag;
            int f = pos.Item1;
            int p = pos.Item2;

            if (poltronas[f, p] == 0)
            {
                DialogResult resp = MessageBox.Show(
                    "Inteira? (Sim) ou Meia? (Não)",
                    "Reservar",
                    MessageBoxButtons.YesNo
                );

                if (resp == DialogResult.Yes)
                {
                    poltronas[f, p] = 1;
                    btn.BackColor = Color.Black;
                }
                else if (resp == DialogResult.No)
                {
                    poltronas[f, p] = 2;
                    btn.BackColor = Color.Gray;
                }
            }
            else
            {
                MessageBox.Show("Assento ocupado");
            }
        }

        private void CalcularFaturamento(object sender, EventArgs e)
        {
            int inteiras = 0;
            int meias = 0;
            decimal total = 0;

            for (int i = 0; i < total_fileiras; i++)
            {
                for (int j = 0; j < total_poltronas; j++)
                {
                    if (poltronas[i, j] == 1)
                    {
                        inteiras++;
                        total += precos[i];
                    }
                    else if (poltronas[i, j] == 2)
                    {
                        meias++;
                        total += precos[i] / 2;
                    }
                }
            }

            lbl_resultado.Text = $"Ocupados: {inteiras + meias} (Inteiras: {inteiras} | Meias: {meias})  Total: R$ {total:F2}";
        }
    }
}
