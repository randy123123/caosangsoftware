$(document).ready(function () {

    /*Add custom ribbon*/
    AddCustomRibbon();

});

function AddCustomRibbon() {
    setTimeout(AddExportItem, 10);
}

function AddExportItem() {
    if($('li[id="CustomRibbon.CustomTab.ExportExcelGroup"]').length == 0)
	{
		$('ul[id="Ribbon.ListItem"]').append('<li unselectable="on" class="ms-cui-group" id="CustomRibbon.CustomTab.ExportExcelGroup"><span unselectable="on" class="ms-cui-groupContainer"><span unselectable="on" class="ms-cui-groupBody"><span unselectable="on" class="ms-cui-layout" id="CustomRibbon.CustomTab.ExportExcelGroup-OneLarge"><span unselectable="on" class="ms-cui-section" id="CustomRibbon.CustomTab.ExportExcelGroup-OneLarge-0"><span unselectable="on" class="ms-cui-row-onerow" id="CustomRibbon.CustomTab.ExportExcelGroup-OneLarge-0-0"><a unselectable="on" href="javascript:;" onclick="javascript:var options = {url: \'/_layouts/SharePointCustomRibbon/ShowItems.aspx?list=\' + SP.ListOperation.Selection.getSelectedList() + \'&amp;view=\' + $(\'[id*=\\\'ListTitleViewSelectorMenu\\\'] span:first\').text() + \'&amp;url=\' + $(location).attr(\'href\'), tite: \'Export to Excel\', allowMaximize: false, showClose: true, width: 300, height: 90 }; SP.UI.ModalDialog.showModalDialog(options); return false;" class="ms-cui-ctl-large " mscui:controltype="Button" role="button" title="Export To Excel" id="CustomRibbon.CustomTab.ExportExcelGroup.ExportToExcelButton-Large"><span unselectable="on" class="ms-cui-ctl-largeIconContainer"><span unselectable="on" class=" ms-cui-img-32by32 ms-cui-img-cont-float"><img unselectable="on" alt="Export To Excel" src="/_layouts/1033/images/formatmap32x32.png" style="top: -352px; left: 0px; "></span></span><span unselectable="on" class="ms-cui-ctl-largelabel">Export To<br>Excel</span></a></span></span></span></span><span unselectable="on" class="ms-cui-groupTitle" title="Export">Export</span></span><span unselectable="on" class="ms-cui-groupSeparator"></span></li>');
	}
    setTimeout(AddExportItem, 10);
}