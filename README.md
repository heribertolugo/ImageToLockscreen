# ImageToLockscreen

**[Currently in Beta. 5/12/2024]**

## TLDR;
Resize images to fit Windows lockscreen. This tool will resize the canvas while preserving the actual image size.

## Background
While trying to use images for my lockscreen on my Windows machine, I found that most images would get cropped or zoomed in. While that can be a nice effect, I purposely wanted the entire image to be visible.
Changing the options in Windows display settings did not help. Prior version of Windows had a resize option that would handle this. The background image does have such a setting, but the lockscreen does not.
To display the images correctly all you have to do is fix the aspect ratio of the image.

## ImageToLockscreen
When selecting images to use as Windows lockscreen, if the image does not have the correct aspect ratio, the image will be rendered zoomed in to fill the entire area of the screen. 
This app will fix the ratio of an image to the desired output size. It will NOT resize the actual image, instead it will overlay the image onto a background that confirms to the ratio desired.
This means the original image will not get distorted due to resizing. There are several choices for what the background can be for the fixed output.


![image](https://github.com/heribertolugo/ImageToLockscreen/assets/26213368/9e648a4d-3630-464c-9acd-b96f312d5dc7)


## Usage

### Install
This app was built using WPF. Meaning it is not a straight forward process to create a portable version.
So the easiest and faster choice to make a release is to publish an installable version, which is what I did.
Unzip the files and then run the setup.exe.
There is no sensible reason or requirement for this app to be installed, so at some point I might look into creating a portable.

### Image Directories
The app does not allow choosing files. 
Instead you must choose the folder which contains the images you would like to fix (**input directory**),
and then choose a folder where the fixed images will be saved to (**output directory**).
The original images are never modifed, and it is up to you (_the user_) to decide if you want to delete or remove them.

### Image Output Options

#### Background Fill
_Fixed_ images will have any empty space filled in using an option of your choice.
The image will be centered on the background.
You can select a solid color as the background fill for the _fixed_ images, or an image.
The image can either be an image of your choice, or the image which is being fixed.
When choosing an image as the background fill, the background fill image **WILL** be resized and distorted.
An optional blur is provided when using an image for background fill. 
The blur will prevent a chaotic looking output, as it allows the main image to retain focus (unless you want the chaotic output, of course).

#### Aspect Ratio
By default the app tries to preselect the aspect ratio which matches your main monitor.
Due to complications with multiple monitors, only the primary is used to determine preselected ratio.
You can however choose any ratio from the option provided. Hovering the mouse over a ratio will show a list of known resolutions to use this ratio.

#### Convert
Once you have the settings the way you want them, click convert and wait. 
Currently the files get coverted to a 96 dpi image, and the images retain the same file type.
In the future I might allow the user to choose between what is detected on your monitors, or the dpi of the image being converted.
**Choosing the blur option will impact performance** quite a bit, making it pretty slow.


## Disclaimer
This program is free to use, and you use it at your own risk. Always make sure to backup any important images just in case.
I can in no way guarantee using this app will not damage you system, cause loss of files or data.
I can in no way be made responsible for any negative outcomes from you downloading or using this app.
I can say I, as the developer, i did test it and use it on my own machine with no negative effects.
My machine is free from malware to my knowledge, and I uploaded the release download straight from the source I uploaded to my repository.
I personally did not include any other code than what is present in the repository or any PUP or Malware.
I personally did upload the source and keep it to its latest state as it is on my machine,
I personally did compile the source on my machine and create a release installer which I uploaded to GitHub releases.
I cannot guarantee the source or any files were not modified after I uploaded.
At the moment I am the only person pushing code or release versions, to my knowledge.
