import { useEffect, useRef, useState } from 'react';
import Button from '@mui/material/Button';
import { Box, Typography } from '@mui/material';

const UploadButton = ({
  doubleClickCallback,
  uploadCallback,
  width,
  maxWidthPerc,
  maxHeightPerc,
  direction,
  image,
  alternativeToNoImage,
  buttonText,
}) => {
  const [imageUrl, setImageUrl] = useState(null);
  const inputRef = useRef();

  maxWidthPerc = maxWidthPerc ?? '50%';
  maxHeightPerc = maxHeightPerc ?? '50%';
  width = width ?? '50%';
  direction = direction ?? 'row';
  buttonText = buttonText ?? 'Upload';

  useEffect(() => {
    if (image) {
      setImageUrl(image);
    }
  }, [image]);

  const handleFileUpload = (event) => {
    try {
      const file = event.target.files[0];
      if (file == null) {
        return;
      }

      uploadCallback(file);

      const reader = new FileReader();
      reader.onloadend = () => {
        setImageUrl(reader.result);
      };

      reader.readAsDataURL(file);
    } catch (e) {
      //console.log(e);
    }
  };

  return (
    <Box
      direction='row'
      display='flex'
      alignItems='center'
      justifyContent={imageUrl == null ? 'center' : 'space-evenly'}
      sx={{
        width: width,
        flexDirection: direction,
        gap: '10px',
      }}
    >
      {imageUrl && (
        <img
          src={imageUrl}
          alt='Uploaded'
          style={{ maxWidth: maxWidthPerc, maxHeight: maxHeightPerc }}
          onDoubleClick={(e) => {
            setImageUrl(null);
            doubleClickCallback(null);
            inputRef.current.value = null;
          }}
        />
      )}
      {alternativeToNoImage && !imageUrl && (
        <Typography>{alternativeToNoImage}</Typography>
      )}
      <label htmlFor='upload-image'>
        <Button variant='contained' component='span'>
          {buttonText}
        </Button>
        <input
          ref={inputRef}
          id='upload-image'
          hidden
          accept='image/*'
          type='file'
          onChange={handleFileUpload}
        />
      </label>
    </Box>
  );
};

export default UploadButton;
