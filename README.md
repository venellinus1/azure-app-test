# Azure KeyVault with ManagedIdentity

Demo:
https://azure-app-test-b6awfjckf0duf2gx.germanywestcentral-01.azurewebsites.net/api/KeyVault

Within **Azure Portal** go to **Home - Create - Key Vault**:

![image](https://github.com/user-attachments/assets/c86814e3-78a6-4047-8589-0d2e774f6c25)

Select **Access Configuration** tab and under **Permission model** select  **Vault access policy**

![image](https://github.com/user-attachments/assets/b4ec69e5-8ff7-46fc-b1af-353ede28d2d7)

 
Under **Access policies** click on **Create** then within the window opened select **Get, List** under **Secret permissions**

![image](https://github.com/user-attachments/assets/4d4533a0-95ef-4e88-bd34-da67357c8954)

Next step is really important - select **Principal** tab and locate your App in the list (eg by name). 
On **Review and Create** tab (that is the 4th step in Create access policy window) verify the **Object ID**  matches your App Object ID.
Your App Obect ID is located at : your app - Settings - Identity - Object (principal) ID

After above steps are completed, go to your Key Vault - Objects - Secrets - click on Generate/Import button to create a new Secret
