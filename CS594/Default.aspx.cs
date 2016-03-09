using System;
using CS594Web.Model;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.Web.Services.Description;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Web.UI;
using System.Diagnostics;
using System.Xml;

namespace CS594Web
{
    public partial class Default : System.Web.UI.Page
    {
        #region :: Events :: 

        /// <summary>
        /// Event for Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pnlError.Visible = false;
                btnHome.Attributes.Add("style", "margin:10px;");
                btnPredefined.Attributes.Add("style", "margin:10px;");
                btnComparison.Attributes.Add("style", "margin:10px;");

                if (!IsPostBack)
                {
                    ddlSavedServices.DataSource = DataAccess.getServices();
                    ddlSavedServices.DataTextField = "STRSERVICE";
                    ddlSavedServices.DataValueField = "STRSERVICEURL";
                    ddlSavedServices.DataBind();
                }

                if (pnlResult.Visible)
                {

                }
                else
                {
                    if (checkURL())
                    {
                        if (Session["methodName"] != null)
                        {
                            getParameters((String)Session["url"], (String)Session["methodName"]);
                        }
                    }
                    else
                    {
                        Session.Clear();
                        getURL();
                        pnlMethods.Visible = false;
                        pnlButtons.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblErrorHeader.Text = ex.Message;
                lblErrorMessage.Text = ex.StackTrace;
                pnlError.Visible = true;
            }

        }

        /// <summary>
        /// On CLick Event for Submit Button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkURL())
                {
                    if (Session["methods"] == null)
                    {
                        getMethods((String)Session["url"]);
                    }
                    else
                    {
                        if (Session["methodName"] == null)
                        {
                            getParameters((String)Session["url"], ddlMethods.SelectedValue);
                            Session["methodName"] = ddlMethods.SelectedValue;
                        }
                        else
                        {
                            getResponse((String)Session["url"]);
                        }
                    }
                }
                else
                {
                    Session.Clear();
                    getURL();
                    getMethods((String)Session["url"]);
                    pnlButtons.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblErrorHeader.Text = ex.Message;
                lblErrorMessage.Text = ex.StackTrace;
                pnlError.Visible = true;
            }
        }

        /// <summary>
        /// Dropdown Value Change for for Results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRecordsCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCompareOne.SelectedValue == "0")
                {
                    getRecords(Convert.ToInt32(ddlRecordsCount.SelectedValue), Convert.ToInt32(ddlRepetataions.SelectedValue), ddlSelectedCategory.SelectedItem.Text);
                }
                else
                {
                    getComperator(Convert.ToInt32(ddlRecordsCount.SelectedValue), Convert.ToInt32(ddlRepetataions.SelectedValue), ddlSelectedCategory.SelectedItem.Text, ddlCompareOne.SelectedItem.Text);
                }
            }
            catch (Exception ex)
            {
                lblErrorHeader.Text = ex.Message;
                lblErrorMessage.Text = ex.StackTrace;
                pnlError.Visible = true;
            }
        }

        /// <summary>
        /// On CLick event for Left Menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMenu_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.ID)
            {
                case "btnHome":
                    //Hide any other Panels.
                    pnlAddNew.Visible = false;
                    pnlResult.Visible = false;
                    pnlMethods.Visible = false;
                    pnlButtons.Visible = false;
                    btnNew.Visible = false;

                    //Handle this panel.
                    Session.Clear();
                    tblButtons.Controls.Clear();
                    ddlSavedServices.ClearSelection();
                    btnSubmit.Visible = true;
                    pnlPredefined.Visible = true;
                    btnSubmit.ValidationGroup = "oldUrl";

                    ddlSavedServices.DataSource = DataAccess.getServices();
                    ddlSavedServices.DataTextField = "STRSERVICE";
                    ddlSavedServices.DataValueField = "STRSERVICEURL";
                    ddlSavedServices.DataBind();
                    break;

                case "btnPredefined":
                    //Hide any other Panels.
                    pnlPredefined.Visible = false;
                    pnlResult.Visible = false;
                    pnlMethods.Visible = false;
                    pnlButtons.Visible = false;
                    btnNew.Visible = false;

                    //Handle main panel
                    Session.Clear();
                    tblButtons.Controls.Clear();
                    txtUrl.Text = string.Empty;
                    btnSubmit.Visible = true;
                    pnlAddNew.Visible = true;
                    btnSubmit.ValidationGroup = "addUrl";
                    break;

                case "btnComparison":
                    //Hide any other Panels.
                    pnlAddNew.Visible = false;
                    pnlPredefined.Visible = false;
                    pnlMethods.Visible = false;
                    pnlButtons.Visible = false;
                    btnSubmit.Visible = false;
                    btnNew.Visible = false;

                    //Handle this Panels.
                    pnlResult.Visible = true;

                    //Code for Dropdown.
                    DataTable dt = DataAccess.getServices();

                    ddlCompareOne.DataSource = dt;
                    ddlCompareOne.DataValueField = "STRSERVICEURL";
                    ddlCompareOne.DataTextField = "STRSERVICE";
                    ddlCompareOne.DataBind();

                    dt.Rows[0][0] = "All";
                    ddlSelectedCategory.DataSource = dt;
                    ddlSelectedCategory.DataValueField = "STRSERVICEURL";
                    ddlSelectedCategory.DataTextField = "STRSERVICE";
                    ddlSelectedCategory.DataBind();

                    ddlRepetataions.DataSource = getRepetations();
                    ddlRepetataions.DataTextField = "Count";
                    ddlRepetataions.DataBind();

                    getRecords(5, 1, "All");
                    break;
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                Session.Clear();
                pnlMethods.Visible = false;
                pnlButtons.Visible = false;
                ddlSavedServices.ClearSelection();
                txtUrl.Text = string.Empty;
                btnSubmit.Visible = true;
                btnNew.Visible = false;
            }
            catch (Exception ex)
            {
                lblErrorHeader.Text = ex.Message;
                lblErrorMessage.Text = ex.StackTrace;
                pnlError.Visible = true;
            }
        }
        #endregion

        #region :: Private Methods ::

        /// <summary>
        /// Get Url from Front End according to the panel.
        /// </summary>
        /// <returns></returns>
        private String getURL()
        {
            try
            {
                String url;

                if (pnlAddNew.Visible)
                {
                    url = txtUrl.Text.Trim();
                }
                else
                {
                    url = ddlSavedServices.SelectedValue;
                }

                Session["url"] = url;

                return url;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Check if the URL has changed with comparison to the one in Session.
        /// </summary>
        /// <returns></returns>
        private bool checkURL()
        {
            try
            {
                if (Session["url"] != null)
                {
                    if (pnlAddNew.Visible)
                    {
                        return txtUrl.Text.Trim().Equals(Session["url"]);
                    }
                    else
                    {
                        return ddlSavedServices.SelectedValue.Trim().Equals(Session["url"]);
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrive the Service Description from url or session.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private ServiceDescription getServiceDescription(String url)
        {
            try
            {
                ServiceDescription sd;
                if (Session["serviceDescription"] == null)
                {
                    WebClient client = new WebClient();
                    Stream str = client.OpenRead(url + "?wsdl");
                    sd = ServiceDescription.Read(str);
                    Session["serviceDescription"] = sd;
                }
                else
                {
                    sd = (ServiceDescription)Session["serviceDescription"];
                }

                return sd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrive methods from the wsdl file.
        /// </summary>
        /// <param name="url"></param>
        private void getMethods(String url)
        {
            try
            {
                //Get Service Description and all the required methods.
                ServiceDescription sd = getServiceDescription(url);
                OperationBindingCollection operations = sd.Bindings[0].Operations;
                tblButtons.Controls.Clear();
                TableRow tRow = new TableRow();

                tRow.Attributes.Add("style", "margin:15px;");

                ddlMethods.DataSource = operations;
                ddlMethods.DataTextField = "Name";
                ddlMethods.DataBind();

                Session["methods"] = operations;

                //cprExtender.Collapsed = false;
                //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "hello", "showMethods();", true);

                pnlMethods.Visible = true;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get Parameters regarding to a specific method from wsdl.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="methodName"></param>
        private void getParameters(String url, String methodName)
        {
            try
            {
                //Get Service Description and required info.
                ServiceDescription sd = getServiceDescription(url);
                ServiceDescriptionImporter importer = new ServiceDescriptionImporter();
                importer.ProtocolName = "Soap12";
                importer.AddServiceDescription(sd, null, null);
                importer.Style = ServiceDescriptionImportStyle.Client;
                importer.CodeGenerationOptions = CodeGenerationOptions.GenerateNewAsync;
                ParameterInfo[] paramArr = null;

                CodeNamespace nm = new CodeNamespace();
                CodeCompileUnit unit = new CodeCompileUnit();
                unit.Namespaces.Add(nm);

                ServiceDescriptionImportWarnings warnings = importer.Import(nm, unit);

                if (warnings == 0)
                {
                    //set the CodeDOMProvider to C# to generate the code in C#
                    System.IO.StringWriter sw = new System.IO.StringWriter();
                    CodeDomProvider provider = CodeDomProvider.CreateProvider("C#");
                    provider.GenerateCodeFromCompileUnit(unit, sw, new CodeGeneratorOptions());
                    string sdName = sd.Services[0].Name;

                    //setting the CompilerParameters for the temporary assembly
                    string[] refAssembly = { "System.dll", "System.Data.dll", "System.Web.Services.dll", "System.Xml.dll" };
                    CompilerParameters param = new CompilerParameters(refAssembly);
                    CompilerResults results = provider.CompileAssemblyFromDom(param, unit);
                    object o = results.CompiledAssembly.CreateInstance(sd.Services[0].Name);
                    Type service = results.CompiledAssembly.GetType(sdName);
                    MethodInfo[] methods = service.GetMethods();

                    Session["instance"] = o;
                    Session["type"] = service;

                    foreach (MethodInfo meth in methods)
                    {
                        if (meth.Name.Equals(methodName))
                        {
                            paramArr = meth.GetParameters();
                            break;
                        }
                    }
                }

                //Code to display selected methods.
                tblButtons.Controls.Clear();

                TableRow tRow = new TableRow();

                TableCell tcLabel = new TableCell();
                TableCell tcType = new TableCell();
                TableCell tcValue = new TableCell();

                tRow.Attributes.Add("style", "margin:15px;");

                Label lblHeader = new Label();
                lblHeader.Text = "Enter Parameters";
                lblHeader.Attributes.Add("style", "margin:10px;");

                pnlButtons.Controls.AddAt(0, lblHeader);

                //Label lblTemp1 = new Label();
                //Label lblTemp2 = new Label();

                //tcLabel.Controls.Add(lblHeader);
                //tcLabel.Controls.Add(lblHeader);
                //tcLabel.Controls.Add(lblHeader);

                //tRow.Controls.Add(tcLabel);
                //tRow.Controls.Add(tcType);
                //tRow.Controls.Add(tcValue);

                //tblButtons.Controls.Add(tRow);

                //to show input for parameters
                foreach (ParameterInfo param in paramArr)
                {
                    tRow = new TableRow();

                    tcLabel = new TableCell();
                    tcType = new TableCell();
                    tcValue = new TableCell();

                    Label lblDescription = new Label();
                    lblDescription.Text = param.Name + " : ";
                    lblDescription.Attributes.Add("style", "margin:10px;");

                    Label lblType = new Label();
                    lblType.Text = param.ParameterType.Name;
                    lblType.CssClass = "col-md-3";
                    lblType.Attributes.Add("style", "margin:10px;");

                    TextBox txtValue = new TextBox();
                    txtValue.ID = param.Name;
                    txtValue.Attributes.Add("style", "margin:10px;");
                    txtValue.CssClass = "form-control";

                    //add control to cells
                    tcLabel.Controls.Add(lblDescription);
                    tcType.Controls.Add(lblType);
                    tcValue.Controls.Add(txtValue);

                    //Add Controls to Row
                    tRow.Controls.Add(tcLabel);
                    tRow.Controls.Add(tcType);
                    tRow.Controls.Add(tcValue);

                    //Add row to table.
                    tblButtons.Controls.Add(tRow);
                }

                //To show a drop down to select the number oftimes the service need to be called.
                tRow = new TableRow();

                tcLabel = new TableCell();
                tcType = new TableCell();
                tcValue = new TableCell();

                Label lblCount = new Label();
                lblCount.Text = "Select Repetations : ";
                lblCount.Attributes.Add("style", "margin:10px;");

                DropDownList ddlCount = new DropDownList();
                ddlCount.ID = "ddlCount";
                ddlCount.Attributes.Add("style", "margin:10px;");
                ddlCount.CssClass = "form-control";

                ddlCount.DataSource = getRepetations();
                ddlCount.DataTextField = "Count";
                ddlCount.DataBind();

                tcLabel.Controls.Add(lblCount);
                tcValue.Controls.Add(ddlCount);

                tRow.Controls.Add(tcLabel);
                tRow.Controls.Add(tcType);
                tRow.Controls.Add(tcValue);

                tblButtons.Controls.Add(tRow);

                pnlButtons.Visible = true;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the method and parameters and invoke it in webservice;
        /// </summary>
        /// <param name="url"></param>
        private void getResponse(String url)
        {
            try
            {
                //Get Service Description and create a parameter array accordingly.
                ServiceDescription sd = getServiceDescription(url);
                string[] paramArray = new string[tblButtons.Controls.Count - 1];
                int intCount = 0;
                int i = 0;

                //Read values entered by each user from each control.
                foreach (Control tableRow in tblButtons.Controls)
                {
                    Control tempControl = tableRow.Controls[2].Controls[0];

                    if (tempControl is TextBox)
                    {
                        paramArray[i] = ((TextBox)tempControl).Text;
                        i++;
                    }

                    if (tempControl is DropDownList)
                    {
                        intCount = Convert.ToInt32(((DropDownList)tempControl).SelectedValue);
                    }
                }

                //Get the instance of the object and other details.
                object o = Session["instance"];
                Type service = (Type)Session["type"];
                Object reader = new object();
                Stopwatch timer = new Stopwatch();

                //Invoke the service.
                timer.Start();
                for (int j = 0; j < intCount; j++)
                {
                    reader = service.InvokeMember((String)Session["methodName"], BindingFlags.InvokeMethod, null, o, paramArray);
                }
                timer.Stop();

                Label lblResult = new Label();
                lblResult.Text = "The Web Service took " + timer.ElapsedMilliseconds.ToString() + " Milliseconds to Complete "
                    + intCount.ToString() + (intCount > 1 ? " calls" : " call");
                lblResult.ForeColor = System.Drawing.ColorTranslator.FromHtml("#d9534f");

                tblButtons.Controls.Clear();
                pnlButtons.Controls.RemoveAt(0);
                pnlButtons.Controls.Add(lblResult);

                ////If theres result from service
                //if (reader != null)
                //{
                //    XmlDocument xmlDoc = new XmlDocument();

                //    //Check if the type of result if String or Object.
                //    if (reader.GetType().Name == "String")
                //    {
                //        xmlDoc.LoadXml((string)reader);
                //    }
                //    else {
                //        XmlSerializer xmlSerial = new XmlSerializer(reader.GetType());


                //        using (MemoryStream xmlStream = new MemoryStream())
                //        {
                //            xmlSerial.Serialize(xmlStream, reader);
                //            xmlStream.Position = 0;
                //            xmlDoc.Load(xmlStream);
                //        }
                //    }

                //    //Code to display the result from Service.

                //    DataTable dt = processResponse(xmlDoc.DocumentElement, 0);

                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        TableRow tr = new TableRow();

                //        for (i = 0; i < Convert.ToInt32(dr["Level"]); i++)
                //        {
                //            TableCell tcBlank = new TableCell();
                //            tr.Cells.Add(tcBlank);
                //        }

                //        TableCell tcData = new TableCell();
                //        tcData.Text = dr["Name"].ToString() + (!dr["Value"].Equals(null) ? " : " + dr["Value"].ToString() : null);
                //        tr.Cells.Add(tcData);

                //        tblButtons.Rows.Add(tr);
                //    }
                //}
                //else
                //{
                //    //Code for Blank Result
                //}

                //Save the result to Database.
                int a = DataAccess.NewRecord(service.FullName, url, intCount, timer.ElapsedMilliseconds);

                btnSubmit.Visible = false;
                btnNew.Visible = true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get records related to single Service.
        /// </summary>
        /// <param name="intCount"></param>
        /// <param name="strService"></param>
        private void getRecords(int intCount, int intTimes, string strService)
        {
            try
            {
                //Get the required info from the database.
                DataTable dtRecords = DataAccess.getRecords(intCount, intTimes, strService).Tables[0];

                if (dtRecords.Rows.Count != 0)
                {
                    string[] x = new string[dtRecords.Rows.Count];
                    decimal[] y = new decimal[dtRecords.Rows.Count];

                    for (int i = 0; i < x.Length; i++)
                    {
                        if (strService == "0")
                        {
                            x[i] = "All";
                        }
                        else
                        {
                            x[i] = dtRecords.Rows[i]["STRService"].ToString();
                        }
                        y[i] = Convert.ToInt32(dtRecords.Rows[i]["INTERVAL"]);
                    }

                    //Bind the data to the BarGraph.
                    bcRecords.ChartTitle = "Speed Test Results for Last " + intCount.ToString() + " Records";
                    bcRecords.Series.Add(new AjaxControlToolkit.BarChartSeries { Data = y, Name = strService, BarColor = "#bbc3cc" });
                    bcRecords.CategoriesAxis = string.Join(",", x);
                }
                else
                {
                    bcRecords.ChartTitle = "There has been no such request before. want to try it now?";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Source to Retrive and Display Data on Bargraph.
        /// </summary>
        /// <param name="intCount"></param>
        /// <param name="strComperatorOne"></param>
        /// <param name="strComperatorTwo"></param>
        private void getComperator(int intCount, int intTimes, string strComperatorOne, string strComperatorTwo)
        {
            try
            {
                //Get information regarding two wwb services.
                DataTable dtRecordsOne = DataAccess.getRecords(intCount, intTimes, strComperatorOne).Tables[0];
                DataTable dtRecordsTwo = DataAccess.getRecords(intCount, intTimes, strComperatorTwo).Tables[0];

                // Check if the web service has no records.
                if (dtRecordsOne.Rows.Count == 0 || dtRecordsTwo.Rows.Count == 0)
                {
                    if (dtRecordsOne.Rows.Count == 0 && dtRecordsTwo.Rows.Count == 0)
                    {
                        bcRecords.ChartTitle = "There has been no such request before. want to try it now?";
                    }
                    else {
                        getRecords(intCount, intTimes, (dtRecordsOne.Rows.Count == 0 ? strComperatorTwo : strComperatorOne));
                    }
                }
                else {
                    //use the info to display on the bar graph.
                    string[] x = new string[(dtRecordsOne.Rows.Count < dtRecordsTwo.Rows.Count ? dtRecordsTwo.Rows.Count : dtRecordsOne.Rows.Count)];
                    decimal[] y = new decimal[dtRecordsOne.Rows.Count];
                    decimal[] z = new decimal[dtRecordsTwo.Rows.Count];

                    for (int i = 0; i < x.Length; i++)
                    {
                        x[i] = (i + 1).ToString();
                    }

                    for (int i = 0; i < dtRecordsOne.Rows.Count; i++)
                    {
                        y[i] = Convert.ToInt32(dtRecordsOne.Rows[i]["INTERVAL"]);
                    }

                    for (int i = 0; i < dtRecordsTwo.Rows.Count; i++)
                    {
                        z[i] = Convert.ToInt32(dtRecordsTwo.Rows[i]["INTERVAL"]);
                    }

                    //bind the data to Bar Graph.
                    bcRecords.ChartTitle = "Speed Test Results for Last " + intCount.ToString() + " Records";
                    bcRecords.Series.Add(new AjaxControlToolkit.BarChartSeries { Data = y, Name = strComperatorOne, BarColor = "#bbc3cc" });
                    bcRecords.Series.Add(new AjaxControlToolkit.BarChartSeries { Data = z, Name = strComperatorTwo, BarColor = "#DE691C" });
                    bcRecords.CategoriesAxis = string.Join(",", x);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// No of times the service should be called.
        /// </summary>
        /// <returns></returns>
        private DataTable getRepetations()
        {
            DataTable dtRepetations = new DataTable();
            try
            {
                //Repetations to be allowed for calling the service.
                int[] a = { 1, 2, 3, 5, 10, 15 };
                int i = 0;
                dtRepetations.Columns.Add("ID");
                dtRepetations.Columns.Add("Count");

                for (i = 0; i < a.Length; i++)
                {
                    DataRow dr = dtRepetations.NewRow();

                    dr["ID"] = i;
                    dr["Count"] = a[i];

                    dtRepetations.Rows.Add(dr);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dtRepetations;
        }

        /// <summary>
        /// Convert to returned XML into DataTable
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="intLevel"></param>
        /// <returns></returns>
        private DataTable processResponse(XmlNode xmlDoc, int intLevel)
        {
            try
            {
                DataTable dtReturn = dataTableStructure();

                //Iterate throught all the nodes and add all the information to a datatable with level details.
                foreach (XmlNode xn in xmlDoc)
                {
                    if (xn.HasChildNodes)
                    {
                        dtReturn.Merge(processResponse(xn, intLevel + 1));
                    }
                    else
                    {
                        DataRow drNode = dtReturn.NewRow();

                        drNode["Name"] = xn.ParentNode.Name;
                        drNode["Value"] = xn.InnerText;
                        drNode["Level"] = intLevel;

                        dtReturn.Rows.Add(drNode);
                    }
                }

                return dtReturn;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Final DataTable Structure to store Result
        /// </summary>
        /// <returns></returns>
        private DataTable dataTableStructure()
        {
            try
            {
                //Create Table with required Columns and return.
                DataTable dt = new DataTable();

                dt.Columns.Add("Name");
                dt.Columns.Add("Value");
                dt.Columns.Add("Level");

                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

    }
}