USE [Xiaoyuyue_2.0]
GO
SET IDENTITY_INSERT [dbo].[WeChatTemplateMessages] ON 

--INSERT [dbo].[WeChatTemplateMessages] ([Id], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [FirstData], [FirstDataColor], [IsActive], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Name], [RemarkData], [RemarkDataColor], [TemplateId], [TemplateIdShort], [Url]) VALUES (6, CAST(N'2016-12-20T02:39:21.1630000' AS DateTime2), NULL, NULL, NULL, N'签到成功提醒', N'#ff9742', 1, 0, NULL, NULL, N'签到成功提醒', N'您已签到成功! 感谢您的使用', N'#ff9742', N'Rhzc05NOpJopDsz64ur05_9vlLsR8Ky51EZP53DM_O4', N'OPENTM207941156', N'%BookingOrder.CheckInURLForCustomer%')

INSERT [dbo].[WeChatTemplateMessages] ([Id], [CreationTime], [CreatorUserId], [DeleterUserId], [DeletionTime], [FirstData], [FirstDataColor], [IsActive], [IsDeleted], [LastModificationTime], [LastModifierUserId], [Name], [RemarkData], [RemarkDataColor], [TemplateId], [TemplateIdShort], [Url]) VALUES (7, CAST(N'2016-12-20T02:39:21.1630000' AS DateTime2), NULL, NULL, NULL, N'您预约的 {%Booking.Name%} 即将开始', N'#ff9742', 1, 0, NULL, NULL, N'预约提醒', N'请提前到达预约地点，并点击此消息进行签到！', N'#ff9742', N'qI3yHk_Jaewf93GTzwnbLWj82GpslBHoO3AwU02utms', N'OPENTM205364973', N'%BookingOrder.CheckInURLForCustomer%')


SET IDENTITY_INSERT [dbo].[WeChatTemplateMessages] OFF
SET IDENTITY_INSERT [dbo].[WeChatTemplateMessageItems] ON 

--INSERT [dbo].[WeChatTemplateMessageItems] ([Id], [Color], [DataName], [DataValue], [TemplateMessageId]) VALUES (15, N'#ff9742', N'keyword1', N'%BookingOrder.CustomerName%', 6)
--INSERT [dbo].[WeChatTemplateMessageItems] ([Id], [Color], [DataName], [DataValue], [TemplateMessageId]) VALUES (16, N'#ff9742', N'keyword2', N'%BookingOrder.CheckInAddress%', 6)
--INSERT [dbo].[WeChatTemplateMessageItems] ([Id], [Color], [DataName], [DataValue], [TemplateMessageId]) VALUES (17, N'#ff9742', N'keyword3', N'%BookingOrder.CheckInTime%', 6)

INSERT [dbo].[WeChatTemplateMessageItems] ([Id], [Color], [DataName], [DataValue], [TemplateMessageId]) VALUES (18, N'#ff9742', N'keyword1', N'%BookingOrder.StartTime%', 7)
INSERT [dbo].[WeChatTemplateMessageItems] ([Id], [Color], [DataName], [DataValue], [TemplateMessageId]) VALUES (19, N'#ff9742', N'keyword2', N'%BookingOrder.EndTime%', 7)

SET IDENTITY_INSERT [dbo].[WeChatTemplateMessageItems] OFF
