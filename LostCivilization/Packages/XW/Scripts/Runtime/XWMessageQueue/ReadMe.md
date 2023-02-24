# 消息队列框架：
**包含两类消息队列：**

1. **AMainThreadMessageQueue**：继承自MonoBehaviour,在Unity主线程中每帧运行
2. **AChildThreadMessageQueue**：需要开启子线程，在没有消息时，子线程会进入睡眠状态，等到有消息时再开始运行。


*注意事项：
1.Unity中的UI内容（MonoBehaviour等）大都只能在主线程中运行;
2.监听者需要主动调用callback()表示事件执行完成，若没有调用，则消息队列会卡在该事件，不会继续执行;*
