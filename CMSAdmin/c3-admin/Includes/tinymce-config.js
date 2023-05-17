﻿var tinyBrowseHeight = 300;
var tinyBrowseWidth = 500;
var tinyBrowseResize = false;

function cmsTinyMceInit(winWidth, winHeight, allowResize) {
	tinyBrowseHeight = parseInt(winHeight);
	tinyBrowseWidth = parseInt(winWidth);
	if (tinyBrowseWidth < 640) {
		tinyBrowseWidth = 640;
	}
	if (tinyBrowseHeight < 175) {
		tinyBrowseHeight = 175;
	}

	tinyBrowseResize = allowResize;

	/*
	menu: {
		file: { title: 'File', items: 'newdocument restoredraft | preview | print ' },
		edit: { title: 'Edit', items: 'undo redo | cut copy paste | selectall | searchreplace' },
		view: { title: 'View', items: 'code | visualaid visualchars visualblocks | spellchecker | preview fullscreen' },
		insert: { title: 'Insert', items: 'image link media template codesample inserttable | charmap emoticons hr | pagebreak nonbreaking anchor toc | insertdatetime' },
		format: { title: 'Format', items: 'bold italic underline strikethrough superscript subscript codeformat | formats blockformats fontformats fontsizes align lineheight | forecolor backcolor | removeformat' },
		tools: { title: 'Tools', items: 'spellchecker spellcheckerlanguage | code wordcount' },
		table: { title: 'Table', items: 'inserttable | cell row column | tableprops deletetable' },
		help: { title: 'Help', items: 'help' }
	}
	 */

	if (tinymce) {
		// because ajax...
		tinymce.remove();
	}

	tinymce.init({
		selector: "textarea.mceEditor",
		file_picker_types: 'file image media',
		file_picker_callback: cmsTinyFileBrowserCallback,
		plugins: 'image imagetools link lists media charmap searchreplace visualblocks paste print table preview code codesample help',
		toolbar1: 'bold italic underline strikethrough sub sup | formatselect forecolor backcolor | blockquote alignleft aligncenter alignright alignjustify outdent indent | help | ',
		toolbar2: 'undo redo searchreplace | bullist numlist | removeformat pastetext | link unlink anchor image media customfilebrowser | charmap codesample code preview visualblocks',
		removed_menuitems: 'newdocument help',
		codesample_languages: [
		   { text: 'HTML', value: 'markup' },
		   { text: 'XML', value: 'xml' },
		   { text: 'Bash', value: 'bash' },
		   { text: 'JavaScript', value: 'javascript' },
		   { text: 'CSS', value: 'css' },
		   { text: 'SQL', value: 'sql' },
		   { text: 'PHP', value: 'php' },
		   { text: 'Ruby', value: 'ruby' },
		   { text: 'Python', value: 'python' },
		   { text: 'Java', value: 'java' },
		   { text: 'C', value: 'c' },
		   { text: 'C#', value: 'csharp' },
		   { text: 'C++', value: 'cpp' }
		],
		resize: tinyBrowseResize,
		width: tinyBrowseWidth,
		height: tinyBrowseHeight,
		relative_urls: false,
		remove_script_host: true,
		extended_valid_elements: "style,link[href|rel]",
		custom_elements: "style,link,~link",
		setup: function (editor) {
			editor.ui.registry.addButton('customfilebrowser', {
				icon: 'document-properties',
				tooltip: 'File Browser',
				onAction: function (_) {
					cmsTinyFileBrowser('0');
				}
			});
		},
		content_css: "/c3-admin/Includes/richedit.css"
	});
}

var lastMetaRequest = null;
var lastCallback = null;

function cmsTinyFileBrowserCallback(callback, value, meta) {
	lastMetaRequest = meta;
	lastCallback = callback;

	cmsTinyFileBrowser('1');
}

function cmsTinyFileBrowser(fld) {
	var sURL = "/c3-admin/FileBrowser.aspx?useTiny=1&returnvalue=" + fld + "&viewmode=file&fldrpath=/";

	tinymce.activeEditor.windowManager.openUrl({
		url: sURL,
		title: 'File Browser',
		resizable: "no",
		scrollbars: "yes",
		status: "yes",
		inline: "yes",
		close_previous: "yes"
	});

	return false;
}

function cmsFileBrowseClose() {
	tinymce.activeEditor.windowManager.close();
}

function cmsFileBrowseSetUri(uri, h, w) {
	if (lastCallback != null) {
		lastCallback(uri);

		lastCallback = null;
		lastMetaRequest = null;
	}

	cmsFileBrowseClose();
}

function cmsPreSaveTrigger() {
	var tgr = tinymce.triggerSave();

	return true;
}

function cmsToggleTinyMCE(id) {
	if (!tinymce.get(id))
		tinymce.execCommand('mceAddControl', false, id);
	else
		tinymce.execCommand('mceRemoveControl', false, id);
}

var fldName = '';
var winBrowse = null;
function cmsFileBrowserOpen(fldN) {
	fldN = '#' + fldN;
	var fld = $(fldN);
	fldName = fld.attr('id');

	if (winBrowse != null) {
		winBrowse.close();
	}

	ShowWindowNoRefresh('/c3-admin/FileBrowser.aspx?useTiny=0&viewmode=file&fldrpath=/');

	return false;
}

function cmsSetFileName(v) {
	var fldN = '#' + fldName;
	var fld = $(fldN);
	fld.val(v);

	winBrowse.close();
	winBrowse = null;
}