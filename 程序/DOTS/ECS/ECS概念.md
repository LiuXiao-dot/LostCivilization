# ECS概念
E:Entity 数据 实体
C:Component 组件 实体中的数据
S:System 系统 控制逻辑
## 专有名词解释
Archetype:有组件组成的组件组原型
Chunk:Archetypes被分为许多非托管内存块，被称为Chunk
World:Entity的组合
EntityManager:负责创建销毁修改World中的Entity
Structural Change:只能在主线程中做，如：增删Archetype中的组件，修改Entity对应的Archetype
Query:查询