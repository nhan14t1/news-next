import AdminLayout from "../../../components/admin/AdminLayout";
import PostManagement from "../../../components/admin/post-management/PostManagement";

function PostManagementPage() {
  return <PostManagement />;
}

PostManagementPage.getLayout = function (page) {
  return <AdminLayout>{page}</AdminLayout>;
}

export default PostManagementPage;