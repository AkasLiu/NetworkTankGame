local sceneManager = class()

function  sceneManager:ctor(  )
	self.envPrefab = Resources.Load("Prefabs/Env")
	self.tankPrefab = Resources.Load("Prefabs/Tank")
	self.shellPrefab = Resources.Load("Prefabs/Shell")
	self.tankExplosionPrefab = Resources.Load("Prefabs/TankExplosion")
	self.gold = Resources.Load("Materials/Gold")
end

function sceneManager:start(  )
	self.env = GameObject.Instantiate(self.envPrefab, Vector3.zero, Quaternion.identity)

	--注册协议
	NetworkManager.Instance:RegisterProto(ProtocolId.SyncPosition, packFunction(self,self.syncPosition))
	NetworkManager.Instance:RegisterProto(ProtocolId.Fire, packFunction(self,self.fireShell))
	NetworkManager.Instance:RegisterProto(ProtocolId.Die, packFunction(self,self.die))
    NetworkManager.Instance:RegisterProto(ProtocolId.Revive, packFunction(self,self.revive))
end

function sceneManager:exit(  )
	--摧毁所有游戏物体
	Camera.main.transform:SetParent(null);

	if next(tanks) ~= nil then
		for k,v in pairs(tanks) do
			if v.gameObject ~= nil then
				GameObject.Destroy(v.gameObject)
			end
		end
		tanks = {}
	end

	GameObject.Destroy(self.env)

	--反注册协议
	NetworkManager.Instance:UnregisterProto(ProtocolId.SyncPosition, packFunction(self,self.syncPosition))
	NetworkManager.Instance:UnregisterProto(ProtocolId.Fire, packFunction(self,self.fireShell))
	NetworkManager.Instance:UnregisterProto(ProtocolId.Die, packFunction(self,self.die))
    NetworkManager.Instance:UnregisterProto(ProtocolId.Revive, packFunction(self,self.revive))
end

function sceneManager:die( param )
	local dieProtocol = Common.Protocol.DieProtocol()
	dieProtocol:Decode(param)

	if dieProtocol.Role_id == userData.id then
		Camera.main.transform:SetParent(null)
		local reviveProtocol = Common.Protocol.ReviveProtocol(dieProtocol.Role_id)
		NetworkManager.Instance:Send(reviveProtocol)
	end

	if tanks[dieProtocol.Role_id] ~= nil then
		local temptank = tanks[dieProtocol.Role_id].gameObject
		local tankExplosion = GameObject.Instantiate(self.tankExplosionPrefab, temptank.transform)
		GameObject.Destroy(tankExplosion,1)
		GameObject.Destroy(temptank)
		tanks[dieProtocol.Role_id] = nil
	end
end

function sceneManager:generateTank( id, customTransform )
	if tanks[id] == nil then
		local position = Vector3(customTransform.X, customTransform.Y, customTransform.Z)
		local rotationVec = Vector3(customTransform.RX, customTransform.RY, customTransform.RZ)	
		local tankGameObject = GameObject.Instantiate(self.tankPrefab, position, Quaternion.Euler(rotationVec))

		local tank = {}
		tank.id = id
		tank.gameObject = tankGameObject
		tank.isDie = false
		tanks[id] = tank

		if userData.id == id then
            Camera.main.transform.parent = tankGameObject.transform
            Camera.main.transform.localPosition = Vector3(0.0, 5.0, -8.0);
            Camera.main.transform.rotation = Quaternion.Euler(Vector3.zero);
            Camera.main.transform:Rotate(20, 0, 0, Space.Self);

            --换肤
            --MeshRenderer[] meshRenerers = self.tankGameObject:GetComponentsInChildren("MeshRenderer")
            --for 
        end
	end
end

function sceneManager:fireShell( param )
	local fireProtocol = Common.Protocol.FireProtocol()
	fireProtocol:Decode(param)
	print(fireProtocol.Role_id)
	print(tanks[fireProtocol.Role_id])
	print(tanks[fireProtocol.Role_id].gameObject)
	local firingTank = tanks[fireProtocol.Role_id].gameObject
	local fireTransform = firingTank.transform:Find("FirePosition")
	local shell = GameObject.Instantiate(self.shellPrefab,fireTransform.position,fireTransform.rotation)
	shell:AddComponent("Shell")
	shell:GetComponent("Shell").dpsd = self.sendDieProtocol
	shell:GetComponent("Rigidbody"):AddForce(firingTank.transform.forward * 1000.0)
end

function sceneManager:syncPosition( param )
	self.syncPositionProtocol = Common.Protocol.SyncPositionProtocol()
	self.syncPositionProtocol:Decode(param)
	self.Role_Id = self.syncPositionProtocol.Role_Id
	self.stf = self.syncPositionProtocol.Stf
	if tanks[self.Role_Id] ~= nil then
		local temptank = tanks[self.Role_Id].gameObject
		temptank.transform.position = Vector3(self.stf.X, self.stf.Y, self.stf.Z)
		temptank.transform:Rotate(0, self.stf.RY - temptank.transform.localEulerAngles.y, 0, Space.Self)
	end
end



function sceneManager:revive( param )
	local reviveProtocol = Common.Protocol.ReviveProtocol()
	reviveProtocol:Decode(param)

	local customTransform = Common.CustomTransform(9.0, -1.0, -7.0, 0.0, 0.0, 0.0)
	self:generateTank(reviveProtocol.Role_id,customTransform)
end

function sceneManager:onRecevieMove( id, hor, ver )
	local direction = Vector3(hor, 0.0, ver)

	local tank = tanks[id].gameObject

	tank.transform:Translate(tank.transform.forward * ver / 10.0 * Time.deltaTime, Space.World)
	tank.transform.transform:Rotate(0.0, hor / 2.0 * Time.deltaTime, 0.0, Space.Self)
end

function sceneManager.sendDieProtocol( collidedObject )
	print("senddie")
	print(collidedObject)
	local role_id = -1
	for k,v in pairs(tanks) do
		if v.gameObject == collidedObject then
			role_id = k
			break
		end
	end
	print("send "..role_id)
	dieProtocol = Common.Protocol.DieProtocol(role_id)
    NetworkManager.Instance:Send(dieProtocol)
end

return sceneManager
