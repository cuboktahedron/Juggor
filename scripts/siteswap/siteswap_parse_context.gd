class_name SiteswapParseContext

var _siteswap: String
var _current := 0

func _init(siteswap: String):
	self._siteswap = siteswap.replace(" ", "")


func peek() -> String:
	if is_eoc():
		return "" 
		
	return _siteswap[_current]


func next() -> String:
	if is_eoc():
		return "" 
	
	var ret = _siteswap[_current]
	_current += 1
	return ret
	

func is_eoc() -> bool:
	return _current >= _siteswap.length()
