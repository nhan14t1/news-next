import { Button, Input, Select } from 'antd';
import React, { useState } from 'react';
import ReactQuill from 'react-quill';
import { CATEGORIES } from '../../../shared/constants/app-const';
import { toSlug } from '../../../shared/utils/stringUtils';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faRedoAlt } from '@fortawesome/free-solid-svg-icons';
import { post } from '../../../shared/utils/apiUtils';
import { useRouter } from 'next/router';

const NewPost = props => {
  const router = useRouter();
  const [postObj, setPostObj] = useState({});

  const categoryOptions = Object.values(CATEGORIES).map(_ => ({ value: _.id, label: _.name }));

  const onTitleBlur = () => {
    if (!postObj.slug && postObj.title && postObj.title.trim()) {
      setPostObj({ ...postObj, slug: toSlug(postObj.title) });
    }
  }
  

  const onCreate = () => {
    if (!validateCreation()) {
      return;
    }

    post('/admin/post', postObj)
      .then(res => {
        router.push('/admin/post-management');
      })
  }

  const validateCreation = () => {
    return true;
  }

  return <div className='new-post'>
    <div className='app-box'>
      <h2>Tạo bài viết</h2>
      <b className='mt-5 d-block'>Tiêu đề:</b>
      <div className='mt-2'>
        <Input key='title' className='w-100' placeholder='Nhập tiêu đề...'
          value={postObj.title}
          onChange={e => setPostObj({ ...postObj, title: e.target.value })}
          onBlur={e => onTitleBlur()}></Input>
      </div>

      <div className='mt-3'>
        <b>Slug</b>&nbsp;&nbsp;
        <Button key="btnSlug" type='primary' size='small' ghost
          disabled={!postObj.title || !postObj.title.trim()}
          onClick={() => setPostObj({ ...postObj, slug: toSlug(postObj.title) })}>
          <FontAwesomeIcon icon={faRedoAlt} />&nbsp;
          Tạo slug
        </Button>
      </div>
      <div className='mt-2'>
        <Input key='slug' className='w-100' placeholder='Ví dụ: an-gi-hom-nay'
          value={postObj.slug}
          onChange={e => setPostObj({ ...postObj, slug: e.target.value })}></Input>
      </div>

      <b className='mt-3 d-block'>Tóm tắt:</b>
      <div className='mt-2'>
        <Input key='introText' className='w-100' placeholder='Giới thiệu vắn tắt để gây sự tò mò'
          value={postObj.introText}
          onChange={e => setPostObj({ ...postObj, introText: e.target.value })}></Input>
      </div>

      <b className='mt-3 d-block'>Categories:</b>
      <div className='mt-2'>
        <Select
          key='categories'
          mode="multiple"
          allowClear
          style={{
            width: '100%',
          }}
          placeholder="Chọn 1 hoặc nhiều category"
          values={postObj.categoryIds || []}
          onChange={(e) => setPostObj({ ...postObj, categoryIds: e })}
          options={categoryOptions}
        />
      </div>

      <b className='mt-3 d-block'>Nhãn (Tùy chọn):</b>
      <div className='mt-2'>
        <Input key='label' className='w-100' placeholder='Chưa có chức năng nhãn'></Input>
      </div>

      <div className='mt-3'><b>Nội dung:</b></div>
      <div className='mt-2'>
        <ReactQuill key='html-editor' theme="snow" value={postObj.content} onChange={(content) => setPostObj({ ...postObj, content })} className='quill-editor' />
      </div>

      <div className='mt-5 d-flex'>
        <div className='ms-auto'>
          <Button key='btn-draft' type='default'>Lưu nháp</Button>
          <Button key='btn-publish' className='ms-2' type='primary'
            onClick={() => onCreate()}>Xuất bản</Button>
          <Button key='btn-schedule' className='ms-2' type='primary' ghost onClick={() => alert('Chức năng này chưa có')}>Lên lịch</Button>
        </div>
      </div>
    </div>
  </div>;
}

export default NewPost;