namespace RaptorTCP3.Forms
{
    partial class frmShowUsers
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.userIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usernameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userPasswordHashDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.registeredDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.countryIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stateIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jurisidictionIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.languagesIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isOnlineDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.accountStatusIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.licenseNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.emailAddressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userClientIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentClientIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.damoclesDataSet = new RaptorTCP3.DamoclesDataSet();
            this.usersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.usersTableAdapter = new RaptorTCP3.DamoclesDataSetTableAdapters.UsersTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.damoclesDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.usersBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.userIdDataGridViewTextBoxColumn,
            this.usernameDataGridViewTextBoxColumn,
            this.userPasswordHashDataGridViewTextBoxColumn,
            this.registeredDateDataGridViewTextBoxColumn,
            this.countryIdDataGridViewTextBoxColumn,
            this.stateIdDataGridViewTextBoxColumn,
            this.jurisidictionIdDataGridViewTextBoxColumn,
            this.languagesIdDataGridViewTextBoxColumn,
            this.isOnlineDataGridViewCheckBoxColumn,
            this.accountStatusIdDataGridViewTextBoxColumn,
            this.licenseNumberDataGridViewTextBoxColumn,
            this.emailAddressDataGridViewTextBoxColumn,
            this.userClientIDDataGridViewTextBoxColumn,
            this.currentClientIDDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.usersBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.GridColor = System.Drawing.Color.DodgerBlue;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(643, 324);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.TabStop = false;
            // 
            // userIdDataGridViewTextBoxColumn
            // 
            this.userIdDataGridViewTextBoxColumn.DataPropertyName = "UserId";
            this.userIdDataGridViewTextBoxColumn.HeaderText = "UserId";
            this.userIdDataGridViewTextBoxColumn.Name = "userIdDataGridViewTextBoxColumn";
            this.userIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.userIdDataGridViewTextBoxColumn.Width = 61;
            // 
            // usernameDataGridViewTextBoxColumn
            // 
            this.usernameDataGridViewTextBoxColumn.DataPropertyName = "Username";
            this.usernameDataGridViewTextBoxColumn.HeaderText = "Username";
            this.usernameDataGridViewTextBoxColumn.Name = "usernameDataGridViewTextBoxColumn";
            this.usernameDataGridViewTextBoxColumn.Width = 78;
            // 
            // userPasswordHashDataGridViewTextBoxColumn
            // 
            this.userPasswordHashDataGridViewTextBoxColumn.DataPropertyName = "UserPasswordHash";
            this.userPasswordHashDataGridViewTextBoxColumn.HeaderText = "UserPasswordHash";
            this.userPasswordHashDataGridViewTextBoxColumn.Name = "userPasswordHashDataGridViewTextBoxColumn";
            this.userPasswordHashDataGridViewTextBoxColumn.Width = 123;
            // 
            // registeredDateDataGridViewTextBoxColumn
            // 
            this.registeredDateDataGridViewTextBoxColumn.DataPropertyName = "RegisteredDate";
            this.registeredDateDataGridViewTextBoxColumn.HeaderText = "RegisteredDate";
            this.registeredDateDataGridViewTextBoxColumn.Name = "registeredDateDataGridViewTextBoxColumn";
            this.registeredDateDataGridViewTextBoxColumn.Width = 104;
            // 
            // countryIdDataGridViewTextBoxColumn
            // 
            this.countryIdDataGridViewTextBoxColumn.DataPropertyName = "CountryId";
            this.countryIdDataGridViewTextBoxColumn.HeaderText = "CountryId";
            this.countryIdDataGridViewTextBoxColumn.Name = "countryIdDataGridViewTextBoxColumn";
            this.countryIdDataGridViewTextBoxColumn.Width = 75;
            // 
            // stateIdDataGridViewTextBoxColumn
            // 
            this.stateIdDataGridViewTextBoxColumn.DataPropertyName = "StateId";
            this.stateIdDataGridViewTextBoxColumn.HeaderText = "StateId";
            this.stateIdDataGridViewTextBoxColumn.Name = "stateIdDataGridViewTextBoxColumn";
            this.stateIdDataGridViewTextBoxColumn.Width = 64;
            // 
            // jurisidictionIdDataGridViewTextBoxColumn
            // 
            this.jurisidictionIdDataGridViewTextBoxColumn.DataPropertyName = "JurisidictionId";
            this.jurisidictionIdDataGridViewTextBoxColumn.HeaderText = "JurisidictionId";
            this.jurisidictionIdDataGridViewTextBoxColumn.Name = "jurisidictionIdDataGridViewTextBoxColumn";
            this.jurisidictionIdDataGridViewTextBoxColumn.Width = 93;
            // 
            // languagesIdDataGridViewTextBoxColumn
            // 
            this.languagesIdDataGridViewTextBoxColumn.DataPropertyName = "LanguagesId";
            this.languagesIdDataGridViewTextBoxColumn.HeaderText = "LanguagesId";
            this.languagesIdDataGridViewTextBoxColumn.Name = "languagesIdDataGridViewTextBoxColumn";
            this.languagesIdDataGridViewTextBoxColumn.Width = 92;
            // 
            // isOnlineDataGridViewCheckBoxColumn
            // 
            this.isOnlineDataGridViewCheckBoxColumn.DataPropertyName = "IsOnline";
            this.isOnlineDataGridViewCheckBoxColumn.HeaderText = "IsOnline";
            this.isOnlineDataGridViewCheckBoxColumn.Name = "isOnlineDataGridViewCheckBoxColumn";
            this.isOnlineDataGridViewCheckBoxColumn.Width = 49;
            // 
            // accountStatusIdDataGridViewTextBoxColumn
            // 
            this.accountStatusIdDataGridViewTextBoxColumn.DataPropertyName = "AccountStatusId";
            this.accountStatusIdDataGridViewTextBoxColumn.HeaderText = "AccountStatusId";
            this.accountStatusIdDataGridViewTextBoxColumn.Name = "accountStatusIdDataGridViewTextBoxColumn";
            this.accountStatusIdDataGridViewTextBoxColumn.Width = 109;
            // 
            // licenseNumberDataGridViewTextBoxColumn
            // 
            this.licenseNumberDataGridViewTextBoxColumn.DataPropertyName = "LicenseNumber";
            this.licenseNumberDataGridViewTextBoxColumn.HeaderText = "LicenseNumber";
            this.licenseNumberDataGridViewTextBoxColumn.Name = "licenseNumberDataGridViewTextBoxColumn";
            this.licenseNumberDataGridViewTextBoxColumn.Width = 104;
            // 
            // emailAddressDataGridViewTextBoxColumn
            // 
            this.emailAddressDataGridViewTextBoxColumn.DataPropertyName = "emailAddress";
            this.emailAddressDataGridViewTextBoxColumn.HeaderText = "emailAddress";
            this.emailAddressDataGridViewTextBoxColumn.Name = "emailAddressDataGridViewTextBoxColumn";
            this.emailAddressDataGridViewTextBoxColumn.Width = 92;
            // 
            // userClientIDDataGridViewTextBoxColumn
            // 
            this.userClientIDDataGridViewTextBoxColumn.DataPropertyName = "UserClientID";
            this.userClientIDDataGridViewTextBoxColumn.HeaderText = "UserClientID";
            this.userClientIDDataGridViewTextBoxColumn.Name = "userClientIDDataGridViewTextBoxColumn";
            this.userClientIDDataGridViewTextBoxColumn.Width = 89;
            // 
            // currentClientIDDataGridViewTextBoxColumn
            // 
            this.currentClientIDDataGridViewTextBoxColumn.DataPropertyName = "CurrentClientID";
            this.currentClientIDDataGridViewTextBoxColumn.HeaderText = "CurrentClientID";
            this.currentClientIDDataGridViewTextBoxColumn.Name = "currentClientIDDataGridViewTextBoxColumn";
            this.currentClientIDDataGridViewTextBoxColumn.Width = 101;
            // 
            // userBindingSource
            // 
            this.userBindingSource.DataSource = typeof(RaptorTCP3.User);
            // 
            // damoclesDataSet
            // 
            this.damoclesDataSet.DataSetName = "DamoclesDataSet";
            this.damoclesDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // usersBindingSource
            // 
            this.usersBindingSource.DataMember = "Users";
            this.usersBindingSource.DataSource = this.damoclesDataSet;
            // 
            // usersTableAdapter
            // 
            this.usersTableAdapter.ClearBeforeFill = true;
            // 
            // frmShowUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 324);
            this.Controls.Add(this.dataGridView1);
            this.Name = "frmShowUsers";
            this.Text = "Show Users";
            this.Load += new System.EventHandler(this.frmShowUsers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.damoclesDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.usersBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn userIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn usernameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userPasswordHashDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn registeredDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn countryIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn stateIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn jurisidictionIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn languagesIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isOnlineDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn accountStatusIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn licenseNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn emailAddressDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userClientIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn currentClientIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource userBindingSource;
        private DamoclesDataSet damoclesDataSet;
        private System.Windows.Forms.BindingSource usersBindingSource;
        private DamoclesDataSetTableAdapters.UsersTableAdapter usersTableAdapter;
    }
}