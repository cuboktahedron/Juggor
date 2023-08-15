extends PopupMenu

signal change_pattern(Array)

const MENU_ID_3 = 0
const MENU_ID_4 = 1
const MENU_ID_1234567 = 2
const MENU_ID_5 = 3


func _on_id_pressed(id):
	if id == MENU_ID_3:
		change_pattern.emit([3])
	elif id == MENU_ID_4:
		change_pattern.emit([4])
	elif id == MENU_ID_1234567:
		change_pattern.emit([1, 2, 3, 4, 5, 6, 7])
	elif id == MENU_ID_5:
		change_pattern.emit([5])
