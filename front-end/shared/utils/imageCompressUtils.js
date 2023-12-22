
import Compress from 'compress.js';

export const compressBase64 = async (base64, size, maxWidth) => {
  let minePrefix = base64.match(/data:.+base64,/)?.[0] || '';
  let mine = minePrefix.replace('data:', '').replace(';base64,', '') || undefined;
  console.log(mine);
  const compressedBase64 = await compressFile(Compress.convertBase64ToFile(base64.replace(minePrefix, ''), mine), size, maxWidth);
  return minePrefix + compressedBase64;
}

export const compressFile = async (file, size = 1, maxWidth = 1920) => {
  var compress = new Compress();

  const resizedImage = await compress.compress([file], {
    size, // the max size in MB, defaults to 2MB
    quality: 1, // the quality of the image, max is 1,
    maxWidth, // the max width of the output image, defaults to 1920px
    maxHeight: 1920, // the max height of the output image, defaults to 1920px
    resize: true // defaults to true, set false if you do not want to resize the image width and height
  })

  const img = resizedImage[0];
  const base64str = img.data;
  return base64str;
  // const imgExt = img.ext
  // const resizedFiile = Compress.convertBase64ToFile(base64str, imgExt)
  // return resizedFiile;
}