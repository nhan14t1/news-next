"use client"
import ReactQuill, { Quill } from 'react-quill';
import ImageUploader from "quill-image-uploader";
import { useMemo } from 'react';
import { postFile } from '../../shared/utils/apiUtils';
import { BASE_IMAGE_URL } from '../../shared/constants/app-const';

Quill.register("modules/imageUploader", ImageUploader);

const HtmlEditor = (props) => {
  const formats = [
    "header",
    "bold",
    "italic",
    "underline",
    "strike",
    "blockquote",
    "list",
    "bullet",
    "indent",
    "link",
    "image",
    "imageBlot"
  ];

  const modules = {
    toolbar: [
      [{ header: [1, 2, false] }],
      ["bold", "italic", "underline", "strike", "blockquote"],
      [
        { list: "ordered" },
        { list: "bullet" },
        { indent: "-1" },
        { indent: "+1" }
      ],
      ["link", "image"],
      ["clean"]
    ],
    imageUploader: {
      upload: (file) => {
        return new Promise((resolve, reject) => {
          const formData = new FormData();
          formData.append("image", file);

          postFile('/admin/file/image', formData)
            .then((result) => {
              let url = `${BASE_IMAGE_URL}/${result.data.name}`;
              resolve(url);
            })
            .catch((error) => {
              reject("Upload failed");
              console.error("Error:", error);
            });
        });
      }
    }
  }

  const uploadImage = async (file) => {

  }

  const moduleMemo = useMemo(() => modules, [])
  const formatMemo = useMemo(() => formats, [])

  return (
    <ReactQuill
      key="quill-html"
      onChange={(html, delta, source) => { console.log(source); source == 'user' && props.onChange(html);}}
      theme="snow"
      className='quill-editor'
      modules={moduleMemo}
      formats={formatMemo}
      value={props.value}
    />
  );
}

export default HtmlEditor;