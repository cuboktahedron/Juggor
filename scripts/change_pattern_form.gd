extends VBoxContainer

signal change_pattern(sp: SiteswapPattern)

@onready var siteswap_txt = $HBox/SiteswapTxt
@onready var error_message_lbl = $ErrorMessage


func _on_button_pressed():
	_submit(siteswap_txt.text)


func _on_siteswap_txt_text_submitted(siteswap: String):
	_submit(siteswap)


func _submit(_siteswap: String):
	var sp := SiteswapPattern.parse(_siteswap)
	if sp.is_valid():
		change_pattern.emit(sp)
		_hide_form()
	else:
		error_message_lbl.text = "Invalid patterns."


func _on_patterns_change_pattern_toggled(is_checked):
	if is_checked:
		siteswap_txt.text = ""
		error_message_lbl.text = ""
		visible = true
		siteswap_txt.grab_focus()
	else:
		_hide_form()


func _hide_form():
	siteswap_txt.text = ""
	error_message_lbl.text = ""
	visible = false
	
