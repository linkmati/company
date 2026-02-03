FCKConfig.DefaultLanguage		= 'en' ;

FCKConfig.Plugins.Add( 'youtube', 'en' ) ;

FCKConfig.ToolbarSets["Site"] = [
	['Cut','Copy','Paste','PasteText','PasteWord'],
	['Undo','Redo','-','Find','Replace','-','SelectAll','RemoveFormat'],
	'/',
	['Bold','Italic','Underline','StrikeThrough','-','Subscript','Superscript'],
	['OrderedList','UnorderedList','-','Outdent','Indent','Blockquote'],
	['JustifyLeft','JustifyCenter','JustifyRight','JustifyFull'],
	['Link','Unlink','Anchor'],
	['Image', 'YouTube','SpecialChar'],
	'/',
	['FontFormat'],
	['Print','SpellCheck','FitWindow']
] ;
FCKConfig.EditorAreaCSS = "/styles/common.css?2009-10-27";
FCKConfig.EditorAreaStyles = "body{background-image:none;background-color: #FFFFFF;padding: 6px 6px 6px 12px;}a{color: #0000ff;}"
FCKConfig.FontFormats	= 'p;h2;h3;h4;h5;pre;address;div' ;