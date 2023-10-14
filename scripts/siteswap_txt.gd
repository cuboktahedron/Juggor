extends LineEdit


var _allowRegEx: RegEx
var _prev_text := ""


func _init():
	_allowRegEx = RegEx.new()
	_allowRegEx.compile("^[0-9a-zA-Z]*$")


func _on_text_changed(new_text: String):
	pass
#	if _allowRegEx.search(new_text) == null:
#		var prev_caret_coumn = caret_column
#		text = _prev_text
#		caret_column = prev_caret_coumn - 1
#	else:
#		var prev_caret_column = caret_column
#		text = new_text.to_upper()
#		caret_column = prev_caret_column
#		_prev_text = text
