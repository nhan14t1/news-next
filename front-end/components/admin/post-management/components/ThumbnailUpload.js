import { faImages, faPlus, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import ImageUploading from 'react-images-uploading';

const ThumbnailUpload = ({ image, onChange }) => {
  const onImageChange = ([image]) => {
    onChange(image ? { base64: image.base64, name: image.file.name } : null);
  };

  return (
    <ImageUploading
      value={image ? [image] : null}
      onChange={onImageChange}
      dataURLKey="base64"
    >
      {({
        imageList,
        onImageUpload,
        onImageRemove,
        isDragging,
        dragProps,
      }) => (
        // write your building UI
        <div className="app-thumbnail-upload">
          {!imageList.length &&
            <div
              style={isDragging ? { color: 'red' } : undefined}
              className='drag-zone'
              onClick={onImageUpload}
              {...dragProps}
            >
              <FontAwesomeIcon icon={faImages} />
            </div>
          }

          {imageList.map((image) => (
            <div key={`img-item`} className="image-item">
              <img src={image['base64']} alt="" className='img-preview' onClick={onImageUpload} />
              <span className='btn-remove' onClick={() => onImageRemove(0)}>
                <FontAwesomeIcon icon={faTrashAlt} />
              </span>
            </div>
          ))}
        </div>
      )}
    </ImageUploading>
  );
}

export default ThumbnailUpload;