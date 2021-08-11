export interface PostDto {
    postContent: string;
    date: Date,
    userId: string;
    postId: string;
    editorName: string;
}

export interface PostCreationDto {
    PostContent: string;
}