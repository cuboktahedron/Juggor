class_name SiteswapSyncHeight

var height: int


func _init(height: int):
	self.height = height


static func parse(context: SiteswapParseContext) -> SiteswapSyncHeight:
	var c = context.peek()
	
	var height := 0
	
	if ("A" <= c and c <= "Z"):
		height = c.unicode_at(0) - "A".unicode_at(0) + 10
	elif "0" <= c and c <= "9":
		height = c.unicode_at(0) - "0".unicode_at(0)
	else:
		return null

	if height % 2 != 0:
		return null

	context.next()
	return SiteswapSyncHeight.new(height)


func label() -> String:
	if height <= 9:
		return str(height)
	else:
		return String.chr("A".unicode_at(0) + height - 10)
