<!--
  Title: ImageToLockscreen
  Description: Make images fit correctly on Windows lock screen.
  Author: Heriberto Lugo
  -->
  <meta name='keywords' content='How to custom-fit your images for the lock screen, set the lock screen wallpaper to fit, get a picture to fit the lockscreen, How to make pic fit the lock screen, How To Make Desktop Background Fit To Screen Windows 10, Lock screen Wallpaper zoomed in how do I fix it?, file convert to ratio'>
  <meta name="description" content="Make images fit correctly on Windows lock screen">
  <meta name="author" content="Heriberto Lugo">

# ImageToLockscreen

**[Currently in Beta. 5/12/2024]**

## TLDR;
Resize images to fit Windows lockscreen. This tool will resize the canvas while preserving the actual image size.

## Background
While trying to use images for my lockscreen on my Windows machine, I found that most images would get cropped or zoomed in. While that can be a nice effect, I purposely wanted the entire image to be visible.
Changing the options in Windows display settings did not help. Prior version of Windows had a resize option that would handle this. The desktop background image does have such a setting, but the lockscreen does not.
To display the images correctly all you have to do is fix the aspect ratio of the image.

## ImageToLockscreen
This app will fix the ratio of an image to the desired output size. It will NOT resize the actual image, instead it will overlay the image onto a background that confirms to the ratio desired.
This means the original image will not get distorted due to resizing. There are several choices for what the background can be for the _"Fixed"_ output image.


![image](https://github.com/heribertolugo/ImageToLockscreen/assets/26213368/9e648a4d-3630-464c-9acd-b96f312d5dc7)


## Usage

### Release/Usage
This app was built using WPF, __.NET 4.8__. You can choose the installer version, or try the portable version, or download the code and compile yourself.
#### Install
1. Go into release from the "Code" tab. Choose a release, and under "Assets", click setup.zip under that release.
Unzip the files and then run the setup.exe.
#### Portable
2. Go into release from the "Code" tab. Choose a release, and under "Assets", click ImageToLockscreen_Portable.zip under that release.
Open ImageToLockscreen.Ui.exe to run the app.
#### Source
3. Go into release from the "Code" tab. Choose a release, and under "Assets", click "_Source code(zip)_" or "_Source code(tar.gz)_" under that release.
Open with __Visual Studio__, and build.

### Image Directories
The app does not allow choosing individual files for fixing. 
Instead, you must choose the folder which contains the images you would like to fix (**input directory**),
and then choose a folder where the fixed images will be saved to (**output directory**).
The original images are never modified, and it is up to you (_the user_) to decide if you want to delete or remove them.

### Image Output Options

#### Background Fill
_"Fixed"_ (converted) images will have any empty space filled in using an option of your choice.
The original image will be centered on the background.
You can select a solid color as the background fill for the _fixed_ images, or an image.
Sample:<br> <br>
**Before**<br> 
<img src="https://github.com/heribertolugo/ImageToLockscreen/assets/26213368/8dd6a79b-5f7f-4f9a-ac20-aec08a605225" width="200"/>
<br><br>**After**<br> 
<img src="https://github.com/heribertolugo/ImageToLockscreen/assets/26213368/31de0a92-0385-466e-9008-bc4d626b78c0" width="400"/>

The image used as the background fill can either be an image of your choice, or the image which is being fixed.
When choosing an image as the background fill, the background fill image **WILL** be resized and distorted.
An optional blur is provided when using an image for background fill. 
The blur will prevent a chaotic looking output, as it allows the main image to retain focus (unless you want the chaotic output, of course).

Sample:<br> <br>
**Before**<br> 
<img src="https://github.com/heribertolugo/ImageToLockscreen/assets/26213368/9e3fd176-9e30-4fb9-8d3a-785efc7a74cf" width="200"/>

<br><br>**After**<br> 
<img src="https://github.com/heribertolugo/ImageToLockscreen/assets/26213368/72e72de1-a3b0-495e-b1e0-27fe1f669523" width="400"/>


#### Aspect Ratio
By default, the app tries to preselect the aspect ratio which matches your main monitor.
Due to complications with multiple monitors, only the primary is used to determine preselected ratio.
You can however choose any ratio from the options provided. Hovering the mouse over a ratio will show a list of known resolutions which use this ratio.

#### Convert
Once you have the settings the way you want them, click convert and wait. 
Currently the files get converted to a 96 dpi image, and the images retain the same file type.
In the future I might allow the user to choose between what is detected on your monitors, or the dpi of the image being converted.
~~**Choosing the blur option will impact performance** quite a bit, making the conversion pretty slow.~~ 
Blur has been greatly improved! 
Converting 315 images took less than 5 minutes on my machine. Most images were almost 2mb and average 2k*4k in size.


## Disclaimer
This program is free to use, and you use it at your own risk. Always make sure to backup any important images just in case.
I can in no way guarantee using this app will not damage your system, cause loss of files or data.
I can in no way be made responsible for any negative outcomes from you downloading or using this app.
I can say I, as the developer, did test the app and use it on my own machine with no negative effects.
My machine is free from malware to my knowledge, and I uploaded the release directly from the source I uploaded to my repository.
I personally did not include any other code than what is present in the repository or any PUP or Malware.
I personally did upload the source and keep it to the latest state as it is on my machine.
I personally did compile the source on my machine and create a release installer which I uploaded to GitHub releases.
I cannot guarantee the source or any files were not modified after I uploaded.
At the moment I am the only person pushing code or release versions, to my knowledge.
