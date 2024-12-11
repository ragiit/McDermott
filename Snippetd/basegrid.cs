 <_BaseMyGrid @ref="Grid"
              Data="@Data"
              @bind-SelectedDataItems="@SelectedDataItems"
              EditModelSaving="OnSave"
              DataItemDeleting="OnDelete"
              FocusedRowChanged="Grid_FocusedRowChanged"
              KeyFieldName="Id">

     <ToolbarTemplate>
         <MyDxToolbarBase TItem="PatientFamilyRelation"
                          SelectedDataItems="@SelectedDataItems"
                          NewItem_Click="@NewItem_Click"
                          EditItem_Click="@EditItem_Click"
                          DeleteItem_Click="@DeleteItem_Click"
                          Refresh_Click="@Refresh_Click"
                          Grid="Grid"
                          ImportFile="ImportFile"
                          ExportToExcel="ExportToExcel"
                          IsImport="UserAccessCRUID.IsImport"
                          VisibleNew="UserAccessCRUID.IsCreate"
                          VisibleEdit="UserAccessCRUID.IsUpdate"
                          VisibleDelete="UserAccessCRUID.IsDelete" />
     </ToolbarTemplate>

     <Columns> 
     </Columns>
 
 </_BaseMyGrid>


private object Data { get; set; }  
private async Task LoadData()
{
    try
    {
        PanelVisible = true;
        SelectedDataItems = []; 
        var dataSource = new GridDevExtremeDataSource<PatientFamilyRelation>(await Mediator.Send(new GetQueryPatientFamilyRelation()))
        {
            CustomizeLoadOptions = (loadOptions) =>
            {
                loadOptions.PrimaryKey = ["Id"];
                loadOptions.PaginateViaPrimaryKey = true;
            }
        };
        Data = dataSource;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastService);
    }
    finally { PanelVisible = false; }
}