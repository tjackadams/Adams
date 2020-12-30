USE [Adams.Service.IdentityDb]
GO

UPDATE [dbo].[ClientRedirectUris]
   SET [RedirectUri] = 'https://localhost:5105/swagger/oauth2-redirect.html'
 WHERE [Id] = 3
GO
