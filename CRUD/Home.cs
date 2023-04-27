using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CRUD
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            cbConsulta.Checked = false;
        }

        private void btnTester_Click(object sender, EventArgs e)
        {
            try{
                using (SqlConnection cn = new SqlConnection(Database.StrCon())){

                    cn.Open();
                    MessageBox.Show("Conexão bem sucedida!");
                }
            }
            catch (Exception ex){
                MessageBox.Show("Falha ao tentar conectar \n\n" + ex.Message);
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            consultarPessoa();
        }

        private void btnIncluir_Click(object sender, EventArgs e){

            InserirPessoa formPessoa = new InserirPessoa(0);

            formPessoa.ShowDialog();

            consultarPessoa();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(dgvQuery.Rows.Count) != 0) { 
                int id = Convert.ToInt32(dgvQuery.Rows[dgvQuery.CurrentCell.RowIndex].Cells[0].Value);

                InserirPessoa formPessoa = new InserirPessoa(id);

                formPessoa.ShowDialog();

                consultarPessoa();
            }
        }

        private void btnDeletar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(dgvQuery.Rows.Count) != 0)
            {
                int id = Convert.ToInt32(dgvQuery.Rows[dgvQuery.CurrentCell.RowIndex].Cells[0].Value);

                InserirPessoa formPessoa = new InserirPessoa(id, true);

                formPessoa.ShowDialog();

                consultarPessoa();
            }
        }

        private void dgvQuery_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Convert.ToInt32(dgvQuery.Rows.Count) != 0)
            {
                int id = Convert.ToInt32(dgvQuery.Rows[dgvQuery.CurrentCell.RowIndex].Cells[0].Value);

                InserirPessoa formPessoa = new InserirPessoa(id);

                formPessoa.ShowDialog();
            }
        }

        private string gerarCondicaoQuery(string texto, Boolean consultaExata)
        {
            string condicao = "";
            if ((texto.Contains(",")) && (consultaExata))
            {
                string[] condicaoVetor = txtSearch.Text.Split(',');

                for (int i = 0; i < condicaoVetor.Length; i++)
                {
                    if (i == 0)
                    {
                        condicao += "'" + condicaoVetor[i] + "'";
                    }
                    else
                        condicao += ", '" + condicaoVetor[i] + "'";
                }

                condicao = "WHERE NOME IN ( " + condicao + ")";
            }
            else if ((texto != "") && (consultaExata))
            {
                condicao = "WHERE NOME = '" + texto + "'";
            }
            else
                condicao = "WHERE NOME LIKE '%" + texto + "%'";

            return condicao;
        }

        private Boolean consultaExata()
        {
            if (cbConsulta.Checked.ToString() == "True")
            {
                return true;
            }
            else
                return false;

        }

        private void cbConsulta_CheckedChanged(object sender, EventArgs e)
        {
            if (consultaExata()){
                MessageBox.Show("Ao marcar essa opção a consulta deverá ser realizada com a vírgula para o caso de busca de multiplos registros.");
            }
        }

        private void consultarPessoa()
        {
            string condicao = gerarCondicaoQuery(txtSearch.Text, consultaExata());

            try
            {
                using (SqlConnection cn = new SqlConnection(Database.StrCon()))
                {
                    cn.Open();

                    var sqlQuery = "SELECT * FROM RH_PESSOA " + condicao;

                    using (SqlDataAdapter da = new SqlDataAdapter(sqlQuery, cn))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            da.Fill(dt);
                            dgvQuery.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Falha ao tentar conectar \n\n" + ex.Message);
            }
        }
    }
}
