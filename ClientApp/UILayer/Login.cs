using ClientApp.RestClientLayer;
using ClientApp.ModelLayer;

namespace ClientApp.UILayer
{
    public partial class LoginForm : Form
    {
        private ApiLoginDataAccess loginApi;
        public LoginForm()
        {
            InitializeComponent();
            loginApi = new ApiLoginDataAccess("https://localhost:7023/api/v1/Login");
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            LoginButtonValidate();
        }

        #region Methods
        //validates the login and decides which view to show: admin or non admin
        public void LoginButtonValidate()
        {
            Login incomingLogin = new Login(textBoxUserName.Text, textBoxPassword.Text);
            if (radioButtonAdmin.Checked)
            {
                incomingLogin.AdminRights = true;
            }
            else if(radioButtonEmployee.Checked)
            {
                incomingLogin.AdminRights = false;
            }
            else
            {
                incomingLogin.AdminRights = false;
            }
            
            bool isValidLogin = loginApi.ValidateLogin(incomingLogin);
            if(isValidLogin == true)
            {
                this.Hide();
                MainForm main = new MainForm(incomingLogin.AdminRights);
                main.ShowDialog();
            }
            else if(isValidLogin == false)
            {
                MessageBox.Show("Invalid login, try again!","Login Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}
