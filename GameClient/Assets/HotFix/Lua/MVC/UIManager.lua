local UIManager = class()

function UIManager:start(  )
	self.UICanvasPrefab = Resources.Load("Prefabs/UI/UICanvas")
	self.UICanvas = GameObject.Instantiate(self.UICanvasPrefab)
end

function UIManager:addView( view )
	view:start()
	self.currentView = view 
end

function UIManager:removeView( view )
	view:destroy()
	self.currentView = nil 
end

function UIManager:update(  )
	self.currentView:update()
end

return UIManager