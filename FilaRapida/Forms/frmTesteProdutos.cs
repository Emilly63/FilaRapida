using System;
using System.Windows.Forms;

namespace FilaRapida.Forms
{
    public class frmTesteProdutos : Form
    {
        private Button btnNovo, btnTeste;
        private DataGridView dgv;
        
        public frmTesteProdutos()
        {
            this.Text = "Teste Rápido";
            this.Size = new System.Drawing.Size(600, 400);
            
            // DataGridView simples
            dgv = new DataGridView();
            dgv.Location = new System.Drawing.Point(20, 20);
            dgv.Size = new System.Drawing.Size(550, 200);
            dgv.Columns.Add("Id", "ID");
            dgv.Columns.Add("Nome", "Nome");
            dgv.Columns.Add("Preco", "Preço");
            
            // Adiciona dados de teste
            dgv.Rows.Add(1, "Produto Teste 1", 10.50);
            dgv.Rows.Add(2, "Produto Teste 2", 25.00);
            
            // Botão NOVO
            btnNovo = new Button();
            btnNovo.Text = "NOVO (Teste)";
            btnNovo.Location = new System.Drawing.Point(20, 250);
            btnNovo.Size = new System.Drawing.Size(100, 30);
            btnNovo.Click += (s, e) => {
                MessageBox.Show("Botão NOVO clicado!", "Teste");
            };
            
            // Botão TESTE
            btnTeste = new Button();
            btnTeste.Text = "TESTE CLIQUE";
            btnTeste.Location = new System.Drawing.Point(150, 250);
            btnTeste.Size = new System.Drawing.Size(100, 30);
            btnTeste.Click += (s, e) => {
                MessageBox.Show("Funciona!", "Sucesso");
            };
            
            this.Controls.AddRange(new Control[] { dgv, btnNovo, btnTeste });
        }
    }
}
