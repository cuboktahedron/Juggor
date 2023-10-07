class_name SiteswapFactor
extends Node

var pattern: String
var height: int


func _init(height: int):
	self.height = height


static func parse(context: String) -> SiteswapFactor:
	var height := 0
	if ("A" <= context and context <= "Z"):
		height = context.unicode_at(0) - "A".unicode_at(0) + 10
	elif "0" <= context and context <= "9":
		height = context.unicode_at(0) - "0".unicode_at(0)
	else:
		return null

	return SiteswapFactor.new(height)


func label():
	if height <= 9:
		return str(height)
	else:
		return String.chr("A".unicode_at(0) + height - 10)
