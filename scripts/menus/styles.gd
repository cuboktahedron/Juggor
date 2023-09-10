extends PopupMenu

signal change_style(style: String)

const MENU_ID_NORMAL = 0
const MENU_ID_REVERSE = 1


func _on_id_pressed(id):
	if MENU_ID_NORMAL <= id and id <= MENU_ID_REVERSE:
		for i in item_count:
			set_item_checked(get_item_id(i), false)
		set_item_checked(id, true)
		change_style.emit(get_item_text(id))
