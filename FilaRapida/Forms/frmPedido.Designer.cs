namespace FilaRapida.Forms
{
    partial class frmPedido
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblAtendente;
        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.Label lblProduto;
        private System.Windows.Forms.ComboBox cboProduto;
        private System.Windows.Forms.Label lblQuantidade;
        private System.Windows.Forms.NumericUpDown numQuantidade;
        private System.Windows.Forms.Label lblPrecoUnitario;
        private System.Windows.Forms.Button btnAdicionar;
        private System.Windows.Forms.DataGridView dgvItens;
        private System.Windows.Forms.Button btnRemover;
        private System.Windows.Forms.Button btnFinalizar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Label lblTotalItens;
        private System.Windows.Forms.Label lblTotalPedido;
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        
        private void InitializeComponent()
        {
            this.lblAtendente = new System.Windows.Forms.Label();
            this.lblData = new System.Windows.Forms.Label();
            this.lblProduto = new System.Windows.Forms.Label();
            this.cboProduto = new System.Windows.Forms.ComboBox();
            this.lblQuantidade = new System.Windows.Forms.Label();
            this.numQuantidade = new System.Windows.Forms.NumericUpDown();
            this.lblPrecoUnitario = new System.Windows.Forms.Label();
            this.btnAdicionar = new System.Windows.Forms.Button();
            this.dgvItens = new System.Windows.Forms.DataGridView();
            this.btnRemover = new System.Windows.Forms.Button();
            this.btnFinalizar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.lblTotalItens = new System.Windows.Forms.Label();
            this.lblTotalPedido = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantidade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).BeginInit();
            this.SuspendLayout();
            
            // lblAtendente
            this.lblAtendente.AutoSize = true;
            this.lblAtendente.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAtendente.Location = new System.Drawing.Point(20, 20);
            this.lblAtendente.Name = "lblAtendente";
            this.lblAtendente.Size = new System.Drawing.Size(90, 17);
            this.lblAtendente.TabIndex = 0;
            this.lblAtendente.Text = "Atendente: ";
            
            // lblData
            this.lblData.AutoSize = true;
            this.lblData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblData.Location = new System.Drawing.Point(20, 45);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(40, 15);
            this.lblData.TabIndex = 1;
            this.lblData.Text = "Data: ";
            
            // lblProduto
            this.lblProduto.AutoSize = true;
            this.lblProduto.Location = new System.Drawing.Point(20, 80);
            this.lblProduto.Name = "lblProduto";
            this.lblProduto.Size = new System.Drawing.Size(47, 13);
            this.lblProduto.TabIndex = 2;
            this.lblProduto.Text = "Produto:";
            
            // cboProduto
            this.cboProduto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProduto.FormattingEnabled = true;
            this.cboProduto.Location = new System.Drawing.Point(20, 100);
            this.cboProduto.Name = "cboProduto";
            this.cboProduto.Size = new System.Drawing.Size(250, 21);
            this.cboProduto.TabIndex = 3;
            
            // lblQuantidade
            this.lblQuantidade.AutoSize = true;
            this.lblQuantidade.Location = new System.Drawing.Point(20, 130);
            this.lblQuantidade.Name = "lblQuantidade";
            this.lblQuantidade.Size = new System.Drawing.Size(65, 13);
            this.lblQuantidade.TabIndex = 4;
            this.lblQuantidade.Text = "Quantidade:";
            
            // numQuantidade
            this.numQuantidade.Location = new System.Drawing.Point(20, 150);
            this.numQuantidade.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numQuantidade.Name = "numQuantidade";
            this.numQuantidade.Size = new System.Drawing.Size(100, 20);
            this.numQuantidade.TabIndex = 5;
            this.numQuantidade.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            
            // lblPrecoUnitario
            this.lblPrecoUnitario.AutoSize = true;
            this.lblPrecoUnitario.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrecoUnitario.ForeColor = System.Drawing.Color.Blue;
            this.lblPrecoUnitario.Location = new System.Drawing.Point(150, 152);
            this.lblPrecoUnitario.Name = "lblPrecoUnitario";
            this.lblPrecoUnitario.Size = new System.Drawing.Size(52, 15);
            this.lblPrecoUnitario.TabIndex = 6;
            this.lblPrecoUnitario.Text = "Preço: ";
            
            // btnAdicionar
            this.btnAdicionar.Location = new System.Drawing.Point(300, 100);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Size = new System.Drawing.Size(100, 70);
            this.btnAdicionar.TabIndex = 7;
            this.btnAdicionar.Text = "Adicionar";
            this.btnAdicionar.UseVisualStyleBackColor = true;
            
            // dgvItens
            this.dgvItens.AllowUserToAddRows = false;
            this.dgvItens.AllowUserToDeleteRows = false;
            this.dgvItens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItens.Location = new System.Drawing.Point(20, 200);
            this.dgvItens.Name = "dgvItens";
            this.dgvItens.ReadOnly = true;
            this.dgvItens.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItens.Size = new System.Drawing.Size(744, 250);
            this.dgvItens.TabIndex = 8;
            
            // Criando colunas para o DataGridView
            var colProduto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colProduto.Name = "colProduto";
            colProduto.HeaderText = "Produto";
            colProduto.DataPropertyName = "Produto.Nome";
            colProduto.Width = 200;
            
            var colQuantidade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colQuantidade.Name = "colQuantidade";
            colQuantidade.HeaderText = "Quantidade";
            colQuantidade.DataPropertyName = "Quantidade";
            colQuantidade.Width = 80;
            
            var colPrecoUnitario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colPrecoUnitario.Name = "colPrecoUnitario";
            colPrecoUnitario.HeaderText = "Preço Unitário";
            colPrecoUnitario.DataPropertyName = "PrecoUnitario";
            colPrecoUnitario.Width = 100;
            
            var colSubtotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSubtotal.Name = "colSubtotal";
            colSubtotal.HeaderText = "Subtotal";
            colSubtotal.DataPropertyName = "Subtotal";
            colSubtotal.Width = 100;
            
            this.dgvItens.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                colProduto, colQuantidade, colPrecoUnitario, colSubtotal});
            
            // btnRemover
            this.btnRemover.Location = new System.Drawing.Point(20, 460);
            this.btnRemover.Name = "btnRemover";
            this.btnRemover.Size = new System.Drawing.Size(100, 30);
            this.btnRemover.TabIndex = 9;
            this.btnRemover.Text = "Remover Item";
            this.btnRemover.UseVisualStyleBackColor = true;
            
            // btnFinalizar
            this.btnFinalizar.BackColor = System.Drawing.Color.LightGreen;
            this.btnFinalizar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFinalizar.Location = new System.Drawing.Point(564, 460);
            this.btnFinalizar.Name = "btnFinalizar";
            this.btnFinalizar.Size = new System.Drawing.Size(100, 40);
            this.btnFinalizar.TabIndex = 10;
            this.btnFinalizar.Text = "Finalizar Pedido";
            this.btnFinalizar.UseVisualStyleBackColor = false;
            
            // btnCancelar
            this.btnCancelar.Location = new System.Drawing.Point(670, 460);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(100, 30);
            this.btnCancelar.TabIndex = 11;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            
            // lblTotalItens
            this.lblTotalItens.AutoSize = true;
            this.lblTotalItens.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalItens.Location = new System.Drawing.Point(130, 468);
            this.lblTotalItens.Name = "lblTotalItens";
            this.lblTotalItens.Size = new System.Drawing.Size(44, 15);
            this.lblTotalItens.TabIndex = 12;
            this.lblTotalItens.Text = "Itens: 0";
            
            // lblTotalPedido
            this.lblTotalPedido.AutoSize = true;
            this.lblTotalPedido.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalPedido.ForeColor = System.Drawing.Color.Green;
            this.lblTotalPedido.Location = new System.Drawing.Point(380, 468);
            this.lblTotalPedido.Name = "lblTotalPedido";
            this.lblTotalPedido.Size = new System.Drawing.Size(140, 20);
            this.lblTotalPedido.TabIndex = 13;
            this.lblTotalPedido.Text = "Total do Pedido:";
            
            // frmPedido
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 511);
            this.Controls.Add(this.lblTotalPedido);
            this.Controls.Add(this.lblTotalItens);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnFinalizar);
            this.Controls.Add(this.btnRemover);
            this.Controls.Add(this.dgvItens);
            this.Controls.Add(this.btnAdicionar);
            this.Controls.Add(this.lblPrecoUnitario);
            this.Controls.Add(this.numQuantidade);
            this.Controls.Add(this.lblQuantidade);
            this.Controls.Add(this.cboProduto);
            this.Controls.Add(this.lblProduto);
            this.Controls.Add(this.lblData);
            this.Controls.Add(this.lblAtendente);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPedido";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Novo Pedido";
            ((System.ComponentModel.ISupportInitialize)(this.numQuantidade)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}