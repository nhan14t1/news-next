import React, { useState } from 'react';
import ReactQuill from 'react-quill';

const NewPost = props => {
    const [value, setValue] = useState('');
    return <ReactQuill theme="snow" value={value} onChange={setValue} />;
} 

export default NewPost;