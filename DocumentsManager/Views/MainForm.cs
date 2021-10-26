using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DocumentsManager.Controllers;
using DocumentsManager.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DocumentsManager.Views
{
    public partial class MainForm : Form
    {

        private MainWindowController controller;

        private BindingSource headersReadBindingSource;
        private BindingSource itemsReadBindingSource;
        private BindingSource headersUpdateBindingSource;
        private BindingSource itemsUpdateBindingSource;
        private BindingSource headersDeleteBindingSource;

        private ErrorProvider errorProvider = new ErrorProvider();

        #region Controls

        private System.Windows.Forms.TabControl tabControl;

        // Create tab
        private LabelX documentHeaderLbl;
        private LabelX headerDateLbl;
        private LabelX clientNumberLbl;
        private LabelX clientNameLbl;
        private LabelX headerNetPriceLbl;
        private LabelX headerGrossPriceLbl;

        private DateTimePicker headerDate;
        private TextBoxX headerClientNumber;
        private TextBoxX headerName;
        private NumericUpDown headerNetPrice;
        private NumericUpDown headerGrossPrice;

        private DataGridViewX headerItems;

        private ButtonX saveDocument;

        // Read tab
        private DataGridViewX headersRead;
        private DataGridViewX headerItemsRead;

        // Update tab
        private DataGridViewX headersUpdate;
        private DataGridViewX headerItemsUpdate;

        // Detele tab
        private DataGridViewX headersDelete;

        #endregion

        public MainForm()
        {
            controller = new MainWindowController();

            headersReadBindingSource = new BindingSource();
            itemsReadBindingSource = new BindingSource();
            headersUpdateBindingSource = new BindingSource();
            itemsUpdateBindingSource = new BindingSource();
            headersDeleteBindingSource = new BindingSource();

            InitializeComponents();
        }

        /// <summary>
        /// Tworzy UI
        /// </summary>
        private void InitializeComponents()
        {
            Size = new Size(1013, 638);
            MaximumSize = MinimumSize = Size;
            CreateSimpleControls();
            CreateContainerControls();
        }

        #region ControlsCreation

        private void CreateSimpleControls()
        {
            // Create
            documentHeaderLbl = new LabelX()
            {
                Name = "lbl1",
                Text = "Nagłówek dokumentu",
                Location = new Point(100, 10),
                Size = new Size(150, 20),
            };
            headerDateLbl = new LabelX()
            {
                Name = "lbl2",
                Text = "Data:",
                Location = new Point(10, 40),
                Size = new Size(150, 20),
            };
            clientNumberLbl = new LabelX()
            {
                Name = "lbl3",
                Text = "Numer klienta:",
                Location = new Point(10, 70),
                Size = new Size(150, 20),
            };
            clientNameLbl = new LabelX()
            {
                Name = "lbl4",
                Text = "Nazwa:",
                Location = new Point(10, 100),
                Size = new Size(150, 20),
            };
            headerNetPriceLbl = new LabelX()
            {
                Name = "lbl5",
                Text = "Cena netto:",
                Location = new Point(10, 130),
                Size = new Size(150, 20),
            };
            headerGrossPriceLbl = new LabelX()
            {
                Name = "lbl6",
                Text = "Cena brutto:",
                Location = new Point(10, 160),
                Size = new Size(150, 20),
            };

            headerDate = new DateTimePicker()
            {
                Name = "headerDate",
                Location = new Point(170, 40),
                Size = new Size(150, 20),
            };
            headerDate.Validating += HeaderDate_Validating;

            headerClientNumber = new TextBoxX()
            {
                Name = "clientNumber",
                Location = new Point(170, 70),
                Size = new Size(150, 20),
            };
            headerClientNumber.Validating += HeaderClientNumber_Validating;

            headerName = new TextBoxX()
            {
                Name = "name",
                Location = new Point(170, 100),
                Size = new Size(150, 20),
            };
            headerName.Validating += HeaderName_Validating;

            headerNetPrice = new NumericUpDown()
            {
                Name = "headerNetPrice",
                Location = new Point(170, 130),
                Size = new Size(150, 20),
                Minimum = 0,
                Maximum = decimal.MaxValue,
                DecimalPlaces = 2,
            };
            headerNetPrice.Validating += HeaderNetPrice_Validating;

            headerGrossPrice = new NumericUpDown()
            {
                Name = "headerGrossPrice",
                Location = new Point(170, 160),
                Size = new Size(150, 20),
                Minimum = 0,
                Maximum = decimal.MaxValue,
                DecimalPlaces = 2,
            };
            headerGrossPrice.Validating += HeaderGrossPrice_Validating;

            headerItems = new DataGridViewX();
            DataGridViewTextBoxColumn artNameColumnCreate = new DataGridViewTextBoxColumn()
            {
                Name = "artNameColCreate",
                HeaderText = "Nazwa artykułu",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            DataGridViewIntegerInputColumn quantityColumnCreate = new DataGridViewIntegerInputColumn()
            {
                Name = "quantityColCreate",
                HeaderText = "Ilość",
                Width = 50,
            };
            DataGridViewDoubleInputColumn netPriceColumnCreate = new DataGridViewDoubleInputColumn()
            {
                Name = "netPriceColCreate",
                HeaderText = "Cena netto"
            };
            DataGridViewDoubleInputColumn grossPriceColumnCreate = new DataGridViewDoubleInputColumn()
            {
                Name = "grossPriceColCreate",
                HeaderText = "Cena brutto"
            };

            headerItems.Location = new Point(350, 10);
            headerItems.Size = new Size(600, 500);
            headerItems.Columns.AddRange(artNameColumnCreate, quantityColumnCreate,
                netPriceColumnCreate, grossPriceColumnCreate);

            saveDocument = new ButtonX();
            saveDocument.Name = "CreateDocumentBtn";
            saveDocument.Text = "Utwórz dokument";
            saveDocument.Location = new Point(10, 520);
            saveDocument.Size = new Size(200, 30);
            saveDocument.Click += SaveDocument_Click;

            // Read
            headersRead = new DataGridViewX()
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                Location = new Point(10, 10),
                Size = new Size(550, 500),
                DataSource = headersReadBindingSource
            };

            headerItemsRead = new DataGridViewX()
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                Location = new Point(570, 10),
                Size = new Size(400, 500),
                DataSource = itemsReadBindingSource
            };

            // Update
            headersUpdate = new DataGridViewX()
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                Location = new Point(10, 10),
                Size = new Size(550, 500),
                DataSource = headersUpdateBindingSource
            };
            headersUpdate.CellValueChanged += HeadersUpdate_CellValueChanged;

            headerItemsUpdate = new DataGridViewX()
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                Location = new Point(570, 10),
                Size = new Size(400, 500),
                DataSource = itemsUpdateBindingSource
            };
            headerItemsUpdate.CellValueChanged += HeaderItemsUpdate_CellValueChanged;

            // Delete
            headersDelete = new DataGridViewX()
            {
                AllowUserToAddRows = false,
                Location = new Point(10, 10),
                ReadOnly = true,
                Size = new Size(550, 500),
                DataSource = headersDeleteBindingSource
            };
            headersDelete.UserDeletingRow += HeadersDelete_UserDeletingRow; ;
        }

        private void CreateContainerControls()
        {
            tabControl = new System.Windows.Forms.TabControl();
            tabControl.Name = "TabControl";
            tabControl.Location = new Point(10, 10);
            tabControl.Size = new Size(980, 580);

            TabPage createPage = new TabPage("Create")
            {
                Name = "TabPage1",
                Size = new Size(980, 580),
            };
            createPage.Controls.AddRange(new Control[]
                {
                    documentHeaderLbl, headerDateLbl, clientNumberLbl,
                    clientNameLbl, headerNetPriceLbl, headerGrossPriceLbl,
                    headerDate, headerClientNumber, headerName, headerNetPrice, headerGrossPrice, headerItems,
                    saveDocument
                });

            tabControl.Controls.Add(createPage);

            TabPage readPage = new TabPage("Read")
            {
                Name = "TabPage2",
                Size = new Size(980, 580),
            };
            readPage.Controls.Add(headersRead);
            readPage.Controls.Add(headerItemsRead);
            readPage.Enter += ReadPage_Enter;

            tabControl.Controls.Add(readPage);

            TabPage updatePage = new TabPage("Update")
            {
                Name = "TabPage3",
                Size = new Size(980, 580),
            };
            updatePage.Controls.Add(headersUpdate);
            updatePage.Controls.Add(headerItemsUpdate);
            updatePage.Enter += UpdatePage_Enter;

            tabControl.Controls.Add(updatePage);

            TabPage deletePage = new TabPage("Delete")
            {
                Name = "TabPage4",
                Size = new Size(980, 580),
            };
            deletePage.Controls.Add(headersDelete);
            tabControl.Controls.Add(deletePage);
            deletePage.Enter += DeletePage_Enter;

            Controls.Add(tabControl);
        }

        #endregion

        #region Events

        private void SaveDocument_Click(object sender, EventArgs e)
        {
            List<DocumentItem> items = new List<DocumentItem>();
            for (int i = 0; i < headerItems.RowCount; i++)
            {
                if (!headerItems.Rows[i].IsNewRow)
                {
                    DocumentItem item = new DocumentItem()
                    {
                        ArtName = Convert.ToString(headerItems.Rows[i].Cells["artNameColCreate"].Value),
                        Quantity = Convert.ToInt32(headerItems.Rows[i].Cells["quantityColCreate"].Value),
                        NetPrice = Convert.ToSingle(headerItems.Rows[i].Cells["netPriceColCreate"].Value),
                        GrossPrice = Convert.ToSingle(headerItems.Rows[i].Cells["grossPriceColCreate"].Value),
                    };

                    items.Add(item);
                }
            }

            DocumentHeader header = new DocumentHeader()
            {
                Date = headerDate.Value,
                ClientNumber = headerClientNumber.Text,
                Name = headerName.Text,
                NetPrice = Convert.ToSingle(headerNetPrice.Value),
                GrossPrice = Convert.ToSingle(headerGrossPrice.Value),
                DocumentItems = items,
            };

            if (controller.SaveDocument(header))
                ClearControls();
        }
        private void ReadPage_Enter(object sender, EventArgs e)
        {
            DataSet data = controller.ReadData();

            headersReadBindingSource.DataSource = data;
            headersReadBindingSource.DataMember = "Headers";

            itemsReadBindingSource.DataSource = headersReadBindingSource;
            itemsReadBindingSource.DataMember = "DocumentItems";

            headersRead.Columns["Id"].Visible = false;
            headersRead.Columns["Date"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            headerItemsRead.Columns["Id"].Visible = headerItemsRead.Columns["DocumentHeaderId"].Visible = false;
            headerItemsRead.Columns["Quantity"].Width = 55;
            headerItemsRead.Columns["ArtName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void UpdatePage_Enter(object sender, EventArgs e)
        {
            DataSet data = controller.ReadData();

            headersUpdateBindingSource.DataSource = data;
            headersUpdateBindingSource.DataMember = "Headers";

            itemsUpdateBindingSource.DataSource = headersUpdateBindingSource;
            itemsUpdateBindingSource.DataMember = "DocumentItems";

            headersUpdate.Columns["Id"].Visible = false;
            headerItemsUpdate.Columns["Id"].Visible = headerItemsUpdate.Columns["DocumentHeaderId"].Visible = false;
            headerItemsUpdate.Columns["Quantity"].Width = 55;

            headersUpdate.Columns["Date"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            headerItemsUpdate.Columns["ArtName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void DeletePage_Enter(object sender, EventArgs e)
        {
            DataSet data = controller.ReadData();

            headersDeleteBindingSource.DataSource = data;
            headersDeleteBindingSource.DataMember = "Headers";

            headersDelete.Columns["Id"].Visible = false;
            headersDelete.Columns["Date"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void HeadersUpdate_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateDocument(sender as DataGridViewX, e.RowIndex, e.ColumnIndex, DocumentHeader.TableName);
        }

        private void HeaderItemsUpdate_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateDocument(sender as DataGridViewX, e.RowIndex, e.ColumnIndex, DocumentItem.TableName);
        }

        private void HeadersDelete_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            int id = (int)e.Row.Cells["Id"].Value;
            controller.DeleteDocument(id);
        }

        #endregion

        #region Validating

        private void HeaderDate_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

            if (sender is DateTimePicker dtPicker)
            {
                if (dtPicker.Value.Year > 2000)
                    e.Cancel = false;
            }

            if (e.Cancel)
                errorProvider.SetError(sender as Control, "Nieprawidłowa wartość.");
            else
                errorProvider.SetError(sender as Control, string.Empty);
        }

        private void HeaderClientNumber_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

            if (sender is TextBoxX cnTextBox)
            {
                if (!string.IsNullOrWhiteSpace(cnTextBox.Text))
                    e.Cancel = false;
            }

            if (e.Cancel)
                errorProvider.SetError(sender as Control, "Nieprawidłowa wartość.");
            else
                errorProvider.SetError(sender as Control, string.Empty);
        }

        private void HeaderName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

            if (sender is TextBoxX nTextBox)
            {
                if (!string.IsNullOrWhiteSpace(nTextBox.Text))
                    e.Cancel = false;
            }

            if (e.Cancel)
                errorProvider.SetError(sender as Control, "Nieprawidłowa wartość.");
            else
                errorProvider.SetError(sender as Control, string.Empty);
        }

        private void HeaderNetPrice_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

            if (sender is NumericUpDown npNumeric)
            {
                if (npNumeric.Value > 0)
                    e.Cancel = false;
            }

            if (e.Cancel)
                errorProvider.SetError(sender as Control, "Nieprawidłowa wartość.");
            else
                errorProvider.SetError(sender as Control, string.Empty);
        }

        private void HeaderGrossPrice_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

            if (sender is NumericUpDown gpNumeric)
            {
                if (gpNumeric.Value > 0)
                    e.Cancel = false;
            }

            if (e.Cancel)
                errorProvider.SetError(sender as Control, "Nieprawidłowa wartość.");
            else
                errorProvider.SetError(sender as Control, string.Empty);
        }

        #endregion

        private void UpdateDocument(DataGridViewX dataGrid, int rowIndex, int columnIndex, string tableName)
        {
            if (dataGrid != null)
            {
                int id = (int)dataGrid.Rows[rowIndex].Cells["Id"].Value;
                object newValue = dataGrid.Rows[rowIndex].Cells[columnIndex].Value;
                string columnName = dataGrid.Columns[columnIndex].Name;

                controller.UpdateDocument(tableName, columnName, id, newValue);
            }
        }

        private void ClearControls()
        {
            headerDate.Value = DateTime.Now;
            headerClientNumber.Text = string.Empty;
            headerName.Text = string.Empty;
            headerNetPrice.Value = 0.0m;
            headerGrossPrice.Value = 0.0m;
            headerItems.Rows.Clear();
        }
    }
}