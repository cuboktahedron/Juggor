extends VBoxContainer

signal change_pattern(Array)

@onready var siteswap_txt = $HBox/SiteswapTxt
@onready var error_message_lbl = $ErrorMessage


func _on_button_pressed():
	_submit(siteswap_txt.text)


func _on_siteswap_txt_text_submitted(siteswap: String):
	_submit(siteswap)


func _submit(_siteswap: String):
	var patterns = _split_tokens(siteswap_txt.text)
	if _is_valid_patterns(patterns):
		change_pattern.emit(patterns)
		_hide_form()
	else:
		error_message_lbl.text = "Invalid patterns."
	

func _split_tokens(siteswap: String) -> Array:
	var patterns = []
	for pattern in siteswap.split(""):
		if "A" <= pattern and pattern <= "Z":
			patterns.push_back(pattern.unicode_at(0) - "A".unicode_at(0) + 10)
		else:
			patterns.push_back(pattern.unicode_at(0) - "0".unicode_at(0))
	
	return patterns


func _is_valid_patterns(patterns: Array) -> bool:
	if patterns.is_empty():
		return false
	
	var sum = 0
	for pattern in patterns:
		sum += pattern
		
	return sum % patterns.size() == 0


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
	
