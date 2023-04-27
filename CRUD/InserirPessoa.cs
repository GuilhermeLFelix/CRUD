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
    public partial class InserirPessoa : Form
    {
        int id = 0;
        public InserirPessoa(int id)
        {
            InitializeComponent();
            btnExcluir.Visible = false;
            this.id = id;

            if (this.id > 0)
            {
                GetPessoa(id);
            }
        }

        public InserirPessoa(int id, bool excluir)
        {
            InitializeComponent();
            this.id = id;
            if (excluir)
            {
                if (this.id > 0)
                {
                    GetPessoa(id);
                    TravarControles();
                    btnExcluir.Visible = true;
                    btnIncluir.Visible = false;
                }
                else
                    this.Close();
            }
        }

        private void TravarControles()
        {
            txtNome.Enabled = false;
            txtId.Enabled = false;
        }

        private void GetPessoa(int id)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.StrCon()))
                {
                    cn.Open();

                    var query = "SELECT * FROM RH_PESSOA WHERE ID_PESSOA = " + id;
                    using (SqlCommand cm = new SqlCommand(query, cn))
                    {
                        using (SqlDataReader dr = cm.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                if (dr.Read())
                                {
                                    txtNome.Text = dr["NOME"].ToString();
                                    txtId.Text = dr["CNPJCPF"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Não foi possível obter a informação do veículo. \n" + ex);
            }
        }

        private void InserirPessoa_Load(object sender, EventArgs e){

        }

        private void btnIncluir_Click(object sender, EventArgs e){

            try
            {
                using (SqlConnection cn = new SqlConnection(Database.StrCon())) {

                    cn.Open();

                    var command = "";

                    if (this.id == 0) 
                    { 
                        command = "INSERT INTO RH_PESSOA(ID_PESSOA, NOME, CNPJCPF) " +
                            "VALUES ((SELECT MAX(ID_PESSOA) + 1 FROM RH_PESSOA), @Nome, @ID)";
                    }
                    else
                    {
                        command = "UPDATE RH_PESSOA SET NOME = @Nome, CNPJCPF = @ID WHERE ID_PESSOA = " + id;
                    }

                    using (SqlCommand cm = new SqlCommand(command, cn)){

                        cm.Parameters.AddWithValue("@Nome", txtNome.Text);
                        cm.Parameters.AddWithValue("@ID", txtId.Text);
                        cm.ExecuteNonQuery();
                    }

                    if (this.id == 0)
                    {
                        MessageBox.Show("Inclusão bem-sucedida!");

                        txtNome.Clear();
                        txtId.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Atualização bem-sucedida!");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível executar a inserção\n" + ex);
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            DialogResult resp = MessageBox.Show("Deseja excluir o registro:", "Excluir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resp == DialogResult.Yes)
            {
                ExcluirPessoa();
                this.Close();
            }
        }

        private void ExcluirPessoa()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Database.StrCon()))
                {
                    cn.Open();

                    var command = "DELETE FROM RH_PESSOA WHERE ID_PESSOA = " + id;

                    using (SqlCommand cmd = new SqlCommand(command, cn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Não foi possível excluir o registro!\n" + ex);
            }
        }
    }
}
