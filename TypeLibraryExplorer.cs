using TLI;

namespace TypeLibraryExplorer
{
    public partial class FrmTypeLibraryExplorer : Form
    {
        private const string Title = "Type Library Explorer";

        private readonly TypeLibInfo tliTypeLibInfo;
        private SearchResults? tliMembers = null;

        public FrmTypeLibraryExplorer()
        {
            InitializeComponent();

            this.Text = Title;

            cdlTypeLibrary.Title = "Open Type Library";
            cdlTypeLibrary.InitialDirectory = @"d:\winnt\system32\";
            cdlTypeLibrary.Filter = "Type Libraries (*.tlb;*.olb;*.dll;*.ocx)|*.tlb;*.olb;*.dll;*.ocx|All Files (*.*)|*.*";
            tliTypeLibInfo = new TypeLibInfo { AppObjString = "<Global>" };

            lvwLibraryInfo.View = View.Details;
            lvwLibraryInfo.FullRowSelect = true;
            lvwLibraryInfo.LabelEdit = false;
            lvwLibraryInfo.LabelWrap = true;
            lvwLibraryInfo.HeaderStyle = ColumnHeaderStyle.None;
            lvwLibraryInfo.Columns.Add(new ColumnHeader { Text = "Property", Width = lvwLibraryInfo.Width / 4 - 4 });
            lvwLibraryInfo.Columns.Add(new ColumnHeader { Text = "Value", Width = lvwLibraryInfo.Width * 3 / 4 });
            lblEntityName.Text = "";
            lblMemberOf.Text = "";
            lblHelpText.Text = "";
            Label3.Visible = false;
        }

        private static string ProduceDefaultValue(object DefVal, TypeInfo tliTypeInfo)
        {
            if (tliTypeInfo == null)
            {
                var defValType = DefVal.GetType();
                if (defValType == typeof(string))
                {
                    if (!string.IsNullOrWhiteSpace(DefVal.ToString()))
                        return "\"" + DefVal + "\"";
                }
                else if (defValType == typeof(bool))
                {
                    return ((bool)DefVal).ToString();
                }
                else if (defValType == typeof(DateTime))
                {
                    if (DefVal != null)
                    {
                        return "#" + DefVal.ToString() + "#";
                    }
                }
                else if (defValType == typeof(int)
                    || defValType == typeof(uint)
                    || defValType == typeof(short)
                    || defValType == typeof(ushort)
                    || defValType == typeof(long)
                    || defValType == typeof(ulong)
                    || defValType == typeof(byte)
                    || defValType == typeof(sbyte)
                    || defValType == typeof(float)
                    || defValType == typeof(double)
                    || defValType == typeof(decimal))
                {
                    if ((int)DefVal != 0)
                    {
                        return ((int)DefVal).ToString();
                    }
                }
                else
                {
                    try
                    {
                        return ((string)DefVal).ToString();
                    }
                    catch { }
                }
            }
            else
            {
                TypeKinds tliTypeKinds = tliTypeInfo.TypeKind;

                while (tliTypeKinds == TypeKinds.TKIND_ALIAS)
                {
                    tliTypeKinds = TypeKinds.TKIND_MAX;
                    try
                    {
                        tliTypeInfo = (TypeInfo)tliTypeInfo.ResolvedType;
                        tliTypeKinds = tliTypeInfo.TypeKind;
                    }
                    catch { }
                }

                if (tliTypeInfo.TypeKind == TypeKinds.TKIND_ENUM)
                {
                    int lngTrackVal = (int)DefVal;
                    foreach (MemberInfo MI in tliTypeInfo.Members)
                    {
                        if (MI.Value == lngTrackVal)
                        {
                            return " = " + MI.Name;
                        }
                    }
                }
            }

            return string.Empty;
        }

        private void MnuFileOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (cdlTypeLibrary.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                tliTypeLibInfo.ContainingFile = cdlTypeLibrary.FileName;

                ProcessTypeLibrary();
            }
            catch (Exception ex)
            {
                if (ex.HResult == (int)TliErrors.tliErrCantLoadLibrary)
                {
                    MessageBox.Show("The file you selected does not contain valid type library information." +
                        Environment.NewLine + "Try a file with the extension *.tlb.", "Invalid Type Library", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static TliSearchTypes GetSearchType(int SearchData)
        {
            if ((SearchData & 0x80000000) != 0)
            {
                return (TliSearchTypes)(((SearchData & 0x7FFFFFFF) / 0x1000000 & 0x7F) | 0x80);
            }
            else
            {
                return (TliSearchTypes)((SearchData / 0x1000000) & 0xFF);
            }
        }

        public string PrototypeMember(int SearchData, InvokeKinds InvokeKinds, string MemberName, int MemberId)
        {
            bool bFirstParameter;
            bool bIsConstant;
            bool bByVal;
            string strTypeName;
            int intVarTypeCur;
            bool bDefault;
            bool bOptional;
            bool bParamArray;
            TypeInfo? tliTypeInfo;
            TypeInfo? tliResolvedTypeInfo;
            TypeKinds tliTypeKinds;
            bool bIsMethod = false;

            // First, determine the type of member we're dealing with
            bIsConstant = GetSearchType(SearchData) == (TliSearchTypes.tliStConstants & GetSearchType(SearchData));
            var memberInfo = tliTypeLibInfo.GetMemberInfo(SearchData, InvokeKinds, MemberId, MemberName);

            string strReturn;
            if (bIsConstant)
            {
                strReturn = "Const ";
            }
            else if (InvokeKinds == InvokeKinds.INVOKE_FUNC || InvokeKinds == InvokeKinds.INVOKE_EVENTFUNC || (int)InvokeKinds == 1610809363)
            {
                bIsMethod = true;
                strReturn = memberInfo.ReturnType.VarType switch
                {
                    TliVarType.VT_VOID or TliVarType.VT_HRESULT => "Sub ",
                    _ => "Function ",
                };
            }
            else
            {
                strReturn = "Property ";
                if ((InvokeKinds & InvokeKinds.INVOKE_PROPERTYGET) == InvokeKinds.INVOKE_PROPERTYGET)
                {
                    strReturn += "Get";
                }
                if ((InvokeKinds & InvokeKinds.INVOKE_PROPERTYPUT) == InvokeKinds.INVOKE_PROPERTYPUT)
                {
                    if (!strReturn.EndsWith(" "))
                    {
                        strReturn += "/";
                    }
                    strReturn += "Let";
                }
                if ((InvokeKinds & InvokeKinds.INVOKE_PROPERTYPUTREF) == InvokeKinds.INVOKE_PROPERTYPUTREF)
                {
                    if (!strReturn.EndsWith(" "))
                    {
                        strReturn += "/";
                    }
                    strReturn += "Set";
                }
                if (!strReturn.EndsWith(" "))
                {
                    strReturn += " ";
                }
            }

            // Now add the name of the member
            strReturn += MemberName;

            // Process the member's parameters
            var parameters = memberInfo.Parameters;
            if (parameters.Count > 0)
            {
                strReturn += "(";
                bFirstParameter = true;
                bParamArray = parameters.OptionalCount == -1;

                foreach (ParameterInfo tliParameterInfo in parameters)
                {
                    if (!bFirstParameter)
                    {
                        strReturn += ", ";
                    }
                    bFirstParameter = false;

                    bDefault = tliParameterInfo.Default;
                    bOptional = bDefault || tliParameterInfo.Optional;

                    if (bOptional)
                    {
                        if (bParamArray)
                        {
                            // This will be the only optional parameter
                            strReturn += "[ParamArray ";
                        }
                        else
                        {
                            strReturn += "[";
                        }
                    }

                    var varTypeInfo = tliParameterInfo.VarTypeInfo;
                    tliTypeInfo = null;
                    tliResolvedTypeInfo = null;
                    tliTypeKinds = TypeKinds.TKIND_MAX;
                    intVarTypeCur = (int)varTypeInfo.VarType;

                    if ((intVarTypeCur & ~((int)TliVarType.VT_ARRAY | (int)TliVarType.VT_VECTOR)) == 0)
                    {
                        try
                        {
                            tliTypeInfo = varTypeInfo.TypeInfo;
                            if (tliTypeInfo != null)
                            {
                                tliResolvedTypeInfo = tliTypeInfo;
                                tliTypeKinds = tliResolvedTypeInfo.TypeKind;
                                while (tliTypeKinds == TypeKinds.TKIND_ALIAS)
                                {
                                    tliTypeKinds = TypeKinds.TKIND_MAX;
                                    try
                                    {
                                        tliResolvedTypeInfo = (TypeInfo)tliResolvedTypeInfo.ResolvedType;
                                        tliTypeKinds = tliResolvedTypeInfo.TypeKind;
                                    }
                                    catch { }
                                }
                            }

                            bByVal = tliTypeKinds switch
                            {
                                TypeKinds.TKIND_INTERFACE or TypeKinds.TKIND_COCLASS or TypeKinds.TKIND_DISPATCH => varTypeInfo.PointerLevel == 1,
                                TypeKinds.TKIND_RECORD => false,// Records not passed ByVal in VB
                                _ => varTypeInfo.PointerLevel == 0,
                            };
                            if (bByVal)
                            {
                                strReturn += "ByVal ";
                            }

                            strReturn += tliParameterInfo.Name;

                            if ((intVarTypeCur & ((int)TliVarType.VT_ARRAY | (int)TliVarType.VT_VECTOR)) != 0)
                            {
                                strReturn += "()";
                            }

                            if (tliTypeInfo == null) // Information not available
                            {
                                strReturn += " As ?";
                            }
                            else
                            {
                                if (varTypeInfo.IsExternalType)
                                {
                                    strReturn += " As " + varTypeInfo.TypeLibInfoExternal.Name + ".";
                                }
                                else
                                {
                                    strReturn += " As ";
                                }
                                strReturn += tliTypeInfo.Name.StartsWith("_") ? tliTypeInfo.Name[1..] : tliTypeInfo.Name;
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        if (varTypeInfo.PointerLevel == 0)
                        {
                            strReturn += "ByVal ";
                        }

                        strReturn += tliParameterInfo.Name;

                        if (intVarTypeCur != (int)TliVarType.VT_VARIANT)
                        {
                            strTypeName = TypeName(varTypeInfo);
                            strReturn += (intVarTypeCur & (int)TliVarType.VT_ARRAY) != 0
                                ? "() As " + strTypeName[..^2]
                                : " As " + strTypeName;
                        }
                    }

                    if (bOptional)
                    {
                        if (bDefault)
                        {
                            strReturn += ProduceDefaultValue(tliParameterInfo.DefaultValue, tliResolvedTypeInfo);
                        }
                        strReturn += "]";
                    }
                }

                strReturn += ")";
            }
            else if (bIsMethod)
            {
                strReturn += "()";
            }

            if (bIsConstant)
            {
                object ConstVal = memberInfo.Value;
                strReturn += " = " + ConstVal;
                var constValType = ConstVal.GetType();
                if (constValType == typeof(int)
                    || constValType == typeof(uint))
                {
                    if (Convert.ToInt32(ConstVal) < 0 || Convert.ToInt32(ConstVal) > 15)
                    {
                        strReturn += " (&H" + Convert.ToString((int)ConstVal, 16) + ")";
                    }
                }
            }
            else
            {
                var returnType = memberInfo.ReturnType;
                intVarTypeCur = (int)returnType.VarType;

                if (intVarTypeCur == 0 || (intVarTypeCur & ~((int)TliVarType.VT_ARRAY | (int)TliVarType.VT_VECTOR)) == 0)
                {
                    try
                    {
                        tliTypeInfo = returnType.TypeInfo;
                        if (tliTypeInfo != null)
                        {
                            if (returnType.IsExternalType)
                            {
                                strReturn += " As " + returnType.TypeLibInfoExternal.Name + ".";
                            }
                            else
                            {
                                strReturn += " As ";
                            }
                            strReturn += tliTypeInfo.Name.StartsWith("_") ? tliTypeInfo.Name[1..] : tliTypeInfo.Name;
                        }

                        if ((intVarTypeCur & ((int)TliVarType.VT_ARRAY | (int)TliVarType.VT_VECTOR)) != 0)
                        {
                            strReturn += "()";
                        }
                    }
                    catch { }
                }
                else
                {
                    switch (intVarTypeCur)
                    {
                        case (int)TliVarType.VT_VARIANT:
                        case (int)TliVarType.VT_VOID:
                        case (int)TliVarType.VT_HRESULT:
                            break;
                        default:
                            strTypeName = TypeName(returnType);
                            strReturn += (intVarTypeCur & (int)TliVarType.VT_ARRAY) != 0
                                ? "() As " + strTypeName[..^2]
                                : " As " + strTypeName;
                            break;
                    }
                }
            }

            lblMemberOf.Text = "Member of " + tliTypeLibInfo.Name + "." + tliTypeLibInfo.GetTypeInfo[SearchData & 0xFFFF].Name;
            lblHelpText.Text = memberInfo.HelpString;

            return strReturn + Environment.NewLine;
        }

        private static string TypeName(VarTypeInfo typeInfo)
        {
            var vtType = (int)typeInfo.VarType;
            var isArray = (vtType & (int)TliVarType.VT_ARRAY) == (int)TliVarType.VT_ARRAY
                || (vtType & (int)TliVarType.VT_VECTOR) == (int)TliVarType.VT_VECTOR;
            vtType &= ~(int)TliVarType.VT_VECTOR;
            vtType &= ~(int)TliVarType.VT_ARRAY;

            string strType = vtType switch
            {
                0 => "Empty",
                1 => "Null",
                2 => "Integer",
                3 => "Long",
                4 => "Single",
                5 => "Double",
                6 => "Currency",
                7 => "Date",
                8 => "String",
                9 => "Object",
                10 => "Error",
                11 => "Boolean",
                12 => "Variant",
                13 => "Object",
                14 => "Currency",
                16 => "Byte",
                17 => "Byte",
                18 => "Integer",
                19 => "Long",
                20 => "Long",// VB6 doesn't have an 8-byte integer type, so we use Long here
                21 => "Long",// VB6 doesn't have an 8-byte integer type, so we use Long here
                22 => "Integer",// VB6 doesn't have a 2-byte integer type, so we use Integer here
                23 => "Integer",// VB6 doesn't have a 2-byte integer type, so we use Integer here
                24 => "Empty",
                25 => "Long",
                26 => "Long",
                27 => "Array",
                28 => "Array",
                29 => "User-defined",
                30 => "String",
                31 => "String",
                36 => "User-defined record",
                64 => "DateTime",
                _ => "Unsupported",
            };
            if (isArray)
            {
                strType += "()";
            }

            return strType;
        }

        private static TliSearchTypes GetSearchType(long SearchData)
        {
            if ((SearchData & 0x80000000) != 0)
            {
                return (TliSearchTypes)(((SearchData & 0x7FFFFFFF) / 0x1000000 & 0x7F) | 0x80);
            }
            else
            {
                return (TliSearchTypes)((SearchData / 0x1000000) & 0xFF);
            }
        }

        private void ProcessTypeLibrary()
        {
            this.Text = Title + " - " + tliTypeLibInfo.Name;

            // Display general type library information in the ListView
            lvwLibraryInfo.Items.Clear();
            lvwLibraryInfo.Items.Add(new ListViewItem(new string[] { "Name", tliTypeLibInfo.Name }));
            lvwLibraryInfo.Items.Add(new ListViewItem(new string[] { "File", tliTypeLibInfo.ContainingFile }));
            lvwLibraryInfo.Items.Add(new ListViewItem(new string[] { "Description", tliTypeLibInfo.HelpString }));
            lvwLibraryInfo.Items.Add(new ListViewItem(new string[] { "Version", tliTypeLibInfo.MajorVersion + "." + tliTypeLibInfo.MinorVersion }));
            lvwLibraryInfo.Items.Add(new ListViewItem(new string[] { "Help File", tliTypeLibInfo.HelpFile }));

            lvwLibraryInfo.Items.Add(new ListViewItem(new string[] { "System", "Unknown" }));
            switch (tliTypeLibInfo.SysKind)
            {
                case SysKinds.SYS_MAC:
                    lvwLibraryInfo.Items[^1].SubItems[1].Text = "Macintosh";
                    break;
                case SysKinds.SYS_WIN16:
                    lvwLibraryInfo.Items[^1].SubItems[1].Text = "Win16";
                    break;
                case SysKinds.SYS_WIN32:
                    lvwLibraryInfo.Items[^1].SubItems[1].Text = "Win32";
                    break;
            }

            lvwLibraryInfo.Items.Add(new ListViewItem(new string[] { "Guid", tliTypeLibInfo.GUID.ToString() }));

            // Clear the ListBox controls
            lstTypeInfos.Items.Clear();
            lstMembers.Items.Clear();

            // Display members for the type library in the ListBox
            var searchResults = tliTypeLibInfo.GetTypes();
            foreach (SearchItem searchResult in searchResults)
            {
                lstTypeInfos.Items.Add(new ListItem { Name = searchResult.Name, Value = searchResult.SearchData });
            }
        }

        private void MnuFileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LstTypeInfos_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Assuming lstTypeInfos is a ListBox control and has a selected item at lstTypeInfos.SelectedIndex
            // lstTypeInfos.[_Default] is the default property of the VB6 ListBox, which returns the selected item's value.
            var selectedItem = (ListItem)lstTypeInfos.SelectedItem;
            lblEntityName.Text = selectedItem.Name;
            //lblEntityName.Text = lstTypeInfos.GetSelectedText();

            // Use the ItemData in lstTypeInfos to set the SearchData for lstMembers
            int searchData = selectedItem.Value;
            //var searchData = lstTypeInfos.GetSelectedItemData();

            // Get the TypeInfo object based on the selected item in lstTypeInfos
            var tliTypeInfo = tliTypeLibInfo.GetTypeInfo[lstTypeInfos.SelectedIndex];

            // Display TypeInfo information
            lblMemberOf.Text = "Member of " + tliTypeInfo.Parent.Name;
            lblHelpText.Text = tliTypeInfo.HelpString;
            txtEntityPrototype.Text = "";

            // Get tliMembers, assuming it's a collection of members for further processing
            tliMembers = tliTypeLibInfo.GetMembers(searchData);

            // Assuming lstMembers is another ListBox control to display members
            lstMembers.Items.Clear();
            var searchResults = tliMembers;
            foreach (SearchItem searchResult in searchResults)
            {
                lstMembers.Items.Add(new ListItem { Name = searchResult.Name, Value = searchResult.MemberId });
            }
        }

        private void TxtEntityPrototype_TextChanged(object sender, EventArgs e)
        {
            Label3.Visible = !string.IsNullOrWhiteSpace(txtEntityPrototype.Text);
        }

        private void LstMembers_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Use the ItemData in lstTypeInfos to set the SearchData for lstMembers
            var selectedItem = (ListItem)lstMembers.SelectedItem;
            var memberId = selectedItem.Value;
            var memberName = selectedItem.Name!;
            lblEntityName.Text = memberName;

            // Get the tliMember based on the selected item in lstMembers
            // Assuming lstMembers has a selected item at lstMembers.SelectedIndex
            SearchItem tliMember = tliMembers![lstMembers.SelectedIndex + 1];
            var tliInvokeKinds = tliMember.InvokeKinds;
            var searchData = ((ListItem)lstTypeInfos.SelectedItem).Value;

            // Assuming lstMembers.[_Default] returns the selected item's value.
            txtEntityPrototype.Text = PrototypeMember(searchData, tliInvokeKinds, memberName, memberId);
        }
    }

    public class ListItem
    {
        public string? Name { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return Name ?? string.Empty;
        }
    }
}