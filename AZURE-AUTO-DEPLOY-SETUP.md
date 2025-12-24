# 🎀 Azure Auto-Deploy Setup Instructions 🎀

## ✅ WHAT I CREATED:

I created a **GitHub Actions workflow** that will automatically deploy your app to **smartgirlalgebra.fun** every time you push to GitHub!

---

## 🔑 YOU NEED TO ADD YOUR AZURE TOKEN (ONE TIME SETUP):

### **STEP 1: Get Your Azure Deployment Token**

1. Go to: https://portal.azure.com
2. Sign in with your Azure account
3. Find your **Static Web App** (search for "SmartGirlAlgebra" or "Static Web Apps")
4. Click on it
5. In the left menu, click **"Overview"**
6. Look for **"Manage deployment token"** button (top area)
7. Click it and **COPY the token** (it's a long string)

---

### **STEP 2: Add Token to GitHub**

1. Go to: https://github.com/maryyon/SmartGirlAlgebra
2. Click **"Settings"** tab (top of the page)
3. In the left sidebar, click **"Secrets and variables"** → **"Actions"**
4. Click **"New repository secret"** button
5. **Name:** `AZURE_STATIC_WEB_APPS_API_TOKEN`
6. **Value:** Paste the token you copied from Azure
7. Click **"Add secret"**

---

## 🚀 THAT'S IT! NOW IT'S AUTOMATIC!

### **From now on:**
- Every time you commit and push in Rider
- GitHub Actions will automatically:
  - ✅ Build your app
  - ✅ Deploy to Azure
  - ✅ Update smartgirlalgebra.fun
  - ✅ Clear caches

### **You can watch it deploy:**
- Go to: https://github.com/maryyon/SmartGirlAlgebra/actions
- You'll see the deployment running!
- Takes about 2-3 minutes

---

## 📋 QUICK REFERENCE:

**Azure Portal:** https://portal.azure.com
**Your GitHub Repo:** https://github.com/maryyon/SmartGirlAlgebra
**Your Live Site:** https://smartgirlalgebra.fun

---

## 🎯 NEXT STEPS:

1. ✅ Push this new workflow file to GitHub (in Rider: Git → Commit → Push)
2. ✅ Get Azure token (Step 1 above)
3. ✅ Add token to GitHub (Step 2 above)
4. ✅ Done! Every push will auto-deploy!

---

🎀 **YOU'RE ALMOST THERE!** 🎀

