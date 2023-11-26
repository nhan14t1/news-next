import AdminLayout from "../../../components/admin/AdminLayout";
import React from 'react';
import dynamic from 'next/dynamic';

const NewPost = dynamic(() => import('../../../components/admin/post-management/NewPost'), { ssr: false });

const NewPostPage = () => {
    return <NewPost/>;
}

NewPostPage.getLayout = function(page) {
    return <AdminLayout>{page}</AdminLayout>;
}

export default NewPostPage;