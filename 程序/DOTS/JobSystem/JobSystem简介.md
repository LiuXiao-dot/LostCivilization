# 简介
添加job后自动进行多线程调度执行。
# 注意
使用Blittable类型的数据：
System.Byte
System.SByte
System.Int16
System.UInt16
System.Int32
System.UInt32
System.Int64
System.IntPtr
System.UIntPtr

其他包中的非托管类型数据:
NativeArray
NativeSlice-NativeArray的子集
TransformAccess
TransformAccessArray
Unity Collection Package中的数据
(需要手动dispose释放)

# 生命周期
Persistent:长生命周期内存
TempJob:只在Job中的短生命周期，4帧以上会警告
Temp:一个函数返回前的短生命周期

# Job调度方式
Run:主线程中立即顺序执行
Schedule:单个工作线程或主线程，每个Job顺序执行
ScheduleParallel:多个工作线程上同时执行，性能最好，但多个工作线程访问同一数据可能会冲突
(本身名字带有Parallel的Job类型，仅提供Schedule,不提供Run和ScheduleParallel的调度方式)

# JobDependencies
两个Job同时修改一个数据，会报错，可以将JobA的句柄handle传递给JobB，保证JobB在JobA之后执行